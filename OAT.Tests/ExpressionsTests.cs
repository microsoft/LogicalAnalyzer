﻿// Copyright (c) Microsoft Corporation. All rights reserved. Licensed under the MIT License.
using Microsoft.CST.OAT.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CST.OAT.Tests
{
    [TestClass]
    public class AnalyzerTests
    {
        [ClassInitialize]
        public static void ClassSetup(TestContext _)
        {
            Logger.SetupVerbose();
            Strings.Setup();
        }

        private TestObject testObjectTrueFalse = new TestObject()
        {
            StringField = "MagicWord",
            BoolField = false
        };

        private TestObject testObjectTrueTrue = new TestObject()
        {
            StringField = "MagicWord",
            BoolField = true
        };

        private TestObject testObjectFalseTrue = new TestObject()
        {
            StringField = "NotTheMagicWord",
            BoolField = true
        };

        private TestObject testObjectFalseFalse = new TestObject()
        {
            StringField = "NotTheMagicWord",
            BoolField = false
        };

        [TestMethod]
        public void TestShortcutWithCapture()
        {
            var rule = new Rule("TestShortcutWithCapture")
            {
                Expression = "1 AND 2 OR 3",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsTrue,"BoolField"){
                        Label = "1",
                        Invert = true
                    },
                    new Clause(Operation.Eq,"StringField"){
                        Label = "2",
                        Data = new List<string>()
                        {
                            "Magic"
                        },
                        Capture = true
                    },
                    new Clause(Operation.IsTrue,"BoolField"){
                        Label = "3"
                    }
                }
            };

            var analyzer = new Analyzer();
            var target = new TestObject()
            {
                StringField = "Magic",
                BoolField = true
            };
            var cap = analyzer.GetCapture(rule, target);
            Assert.IsTrue(cap.Result?.Captures.First() is TypedClauseCapture<string> t && t.Result == "Magic");
        }

        [TestMethod]
        public void TestShortcut()
        {
            var rule = new Rule("TestShortcut")
            {
                Expression = "1 AND 2 OR 3",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsTrue,"BoolField"){
                        Label = "1",
                        Invert = true
                    },
                    new Clause(Operation.Custom,"StringField"){
                        Label = "2"
                    },
                    new Clause(Operation.IsTrue,"BoolField"){
                        Label = "3"
                    }
                }
            };

            var rule2 = new Rule("TestShortcut")
            {
                Expression = "1 OR 2",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsTrue,"BoolField"){
                        Label = "1",
                    },
                    new Clause(Operation.Custom,"StringField"){
                        Label = "2",
                    }
                }
            };

            var analyzer = new Analyzer();

            analyzer.CustomOperationDelegates.Add((Clause _, object? __, object? ___) =>
            {
                // We should shortcut calling the custom operation entirely, because it is not being captured
                // Given the test data this line should never be hit
                Assert.Fail();
                return (false, false, null);
            });
            var target = new TestObject()
            {
                StringField = "Magic",
                BoolField = true
            };
            var cap = analyzer.GetCapture(rule, target);
            cap = analyzer.GetCapture(rule2, target);
        }

        [TestMethod]
        public void TestNotNot()
        {
            var RuleName = "Not Not True";
            var notNotRule = new Rule(RuleName)
            {
                Expression = "NOT NOT 0",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "StringField")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "MagicWord"
                        }
                    }
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { notNotRule };

            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectTrueFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectTrueTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectFalseFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectFalseTrue).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void TestXorFromNand()
        {
            var RuleName = "XOR from NAND";
            var xorRule = new Rule(RuleName)
            {
                Expression = "(0 NAND (0 NAND 1)) NAND (1 NAND (0 NAND 1))",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "StringField")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "MagicWord"
                        }
                    },
                    new Clause(Operation.IsTrue, "BoolField")
                    {
                        Label = "1"
                    }
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { xorRule };

            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectTrueFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectTrueTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectFalseFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectFalseTrue).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void VerifyAccessSubproperties()
        {
            var regObj = new TestObject()
            {
                StringDictField = new Dictionary<string, string>()
                {
                    { "One", "Two" }
                }
            };

            var RuleName = "ContainsRule";
            var containsRule = new Rule(RuleName)
            {
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "StringDictField.One")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "Two"
                        }
                    }
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { containsRule };
            Assert.IsTrue(analyzer.Analyze(ruleList, regObj).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void VerifyImplicitAndWithInvert()
        {
            var RuleName = "ImplicitAndWithInvert";

            var implicitAndWithInvert = new Rule(RuleName)
            {
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "StringField")
                    {
                        Data = new List<string>()
                        {
                            "MagicWord"
                        },
                        Invert = true
                    },
                    new Clause(Operation.IsTrue, "BoolField")
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { implicitAndWithInvert };

            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectFalseFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectFalseTrue).Any(x => x.Name == RuleName)); // The first clause is inverted
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectTrueTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectTrueFalse).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void VerifyImplicitAnd()
        {
            var RuleName = "ImplicitAnd";

            var implicitAnd = new Rule(RuleName)
            {
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "StringField")
                    {
                        Data = new List<string>()
                        {
                            "MagicWord"
                        }
                    },
                    new Clause(Operation.IsTrue, "BoolField")
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { implicitAnd };

            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectFalseFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectFalseTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectTrueTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectTrueFalse).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void VerifyAnd()
        {
            var RuleName = "AndRule";
            var andRule = new Rule(RuleName)
            {
                Expression = "0 AND 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "StringField")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "MagicWord"
                        }
                    },
                    new Clause(Operation.IsTrue, "BoolField")
                    {
                        Label = "1"
                    }
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { andRule };

            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectFalseFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectFalseTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectTrueTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectTrueFalse).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void VerifyInvalidRuleDetection()
        {
            var invalidRule = new Rule("Unbalanced Parentheses")
            {
                Expression = "( 0 AND 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "Path")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "TestPath2"
                        }
                    },
                    new Clause(Operation.Eq, "IsExecutable")
                    {
                        Label = "1",
                        Data = new List<string>()
                        {
                            "True"
                        }
                    }
                }
            };
            var analyzer = new Analyzer();
            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("ClauseInParenthesesLabel")
            {
                Expression = "WITH(PARENTHESIS)",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "WITH(PARENTHESIS)"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("CharactersBetweenParentheses")
            {
                Expression = "(W(I",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "W(I"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("CharactersBeforeOpenParentheses")
            {
                Expression = "W(I",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "W(I"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("CharactersBetweenClosedParentheses")
            {
                Expression = "(0 AND W)I)",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "W)I"
                    },
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "0"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("CharactersAfterClosedParentheses")
            {
                Expression = "0 AND W)I",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "W)I"
                    },
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "0"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("CloseParenthesesWithNot")
            {
                Expression = "(0 AND NOT) 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "1"
                    },
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "0"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("WhiteSpaceLabel")
            {
                Expression = "0 AND   ",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "0"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("InvalidOperator")
            {
                Expression = "0 XAND 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "1"
                    },
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "0"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("InvalidNotOperator")
            {
                Expression = "0 NOT AND 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "1"
                    },
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "0"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("EndsWithOperator")
            {
                Expression = "0 AND",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "0"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("UnusedLabel")
            {
                Expression = "0",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "1"
                    },
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "0"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("MissingLabel")
            {
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "0"
                    },
                    new Clause(Operation.IsNull, "Path")
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("ExpressionRequiresLabels")
            {
                Expression = "0 AND 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path"),
                    new Clause(Operation.IsNull, "Path")
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("OutOfOrder")
            {
                Expression = "0 1 AND",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "Path")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "TestPath2"
                        }
                    },
                    new Clause(Operation.IsTrue, "IsExecutable")
                    {
                        Label = "1"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("StartWithOperator")
            {
                Expression = "OR 0 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "Path")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "TestPath2"
                        }
                    },
                    new Clause(Operation.Eq, "IsExecutable")
                    {
                        Label = "1",
                        Data = new List<string>()
                        {
                            "True"
                        }
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("Case Sensitivity")
            {
                Expression = "Variable",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "VARIABLE"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("OPERATION.Custom without CustomOperation")
            {
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Custom, "Path")
                    {
                        Label = "VARIABLE"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));

            invalidRule = new Rule("OPERATION.Custom without validation Delegate")
            {
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Custom, "Path")
                    {
                        Label = "VARIABLE",
                        CustomOperation = "NO_DELEGATE"
                    }
                }
            };

            Assert.IsFalse(analyzer.IsRuleValid(invalidRule));
        }

        [TestMethod]
        public void VerifyNand()
        {
            var RuleName = "NandRule";
            var nandRule = new Rule(RuleName)
            {
                Expression = "0 NAND 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "StringField")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "MagicWord"
                        }
                    },
                    new Clause(Operation.IsTrue, "BoolField")
                    {
                        Label = "1"
                    }
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { nandRule };

            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectTrueTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectTrueFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectFalseTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectFalseFalse).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void VerifyNor()
        {
            var RuleName = "NorRule";
            var norRule = new Rule(RuleName)
            {
                Expression = "0 NOR 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "StringField")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "MagicWord"
                        }
                    },
                    new Clause(Operation.IsTrue, "BoolField")
                    {
                        Label = "1"
                    }
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { norRule };

            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectFalseTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectFalseFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectTrueFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectTrueTrue).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void VerifyCustom()
        {
            var RuleName = "CustomRule";
            var customRule = new Rule(RuleName)
            {
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Custom, "StringField")
                    {
                        CustomOperation = "RETURN_TRUE",
                        Data = new List<string>()
                        {
                            "TestPath1"
                        }
                    },
                }
            };

            var analyzer = new Analyzer();

            analyzer.CustomOperationDelegates.Add((clause, before, after) =>
            {
                if (clause.Operation == Operation.Custom)
                {
                    if (clause.CustomOperation == "RETURN_TRUE")
                    {
                        return (true, true, null);
                    }
                }
                return (false, false, null);
            });

            var ruleList = new List<Rule>() { customRule };

            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectTrueTrue).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void VerifyNot()
        {
            var RuleName = "NotRule";
            var notRule = new Rule(RuleName)
            {
                Expression = "NOT 0",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "StringField")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "MagicWord"
                        }
                    }
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { notRule };

            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectTrueTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectFalseFalse).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void VerifyOr()
        {
            var RuleName = "OrRule";
            var orRule = new Rule(RuleName)
            {
                Expression = "0 OR 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "StringField")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "MagicWord"
                        }
                    },
                    new Clause(Operation.IsTrue, "BoolField")
                    {
                        Label = "1"
                    }
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { orRule };

            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectTrueTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectTrueFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectFalseTrue).Any(x => x.Name == RuleName));
            Assert.IsFalse(analyzer.Analyze(ruleList, testObjectFalseFalse).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void VerifyValidRuleDetection()
        {
            var validRule = new Rule("Regular Rule")
            {
                Expression = "0 AND 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "Path")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "TestPath2"
                        }
                    },
                    new Clause(Operation.Eq, "IsExecutable")
                    {
                        Label = "1",
                        Data = new List<string>()
                        {
                            "True"
                        }
                    }
                }
            };

            var analyzer = new Analyzer();
            Assert.IsTrue(analyzer.IsRuleValid(validRule));

            validRule = new Rule("Extraneous Parenthesis")
            {
                Expression = "(0 AND 1)",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "Path")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "TestPath2"
                        }
                    },
                    new Clause(Operation.Eq, "IsExecutable")
                    {
                        Label = "1",
                        Data = new List<string>()
                        {
                            "True"
                        }
                    }
                }
            };

            Assert.IsTrue(analyzer.IsRuleValid(validRule));

            validRule = new Rule("Deeply Nested Expression")
            {
                Expression = "(0 AND 1) OR (2 XOR (3 AND (4 NAND 5)) OR 6)",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "Path")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "TestPath2"
                        }
                    },
                    new Clause(Operation.Eq, "IsExecutable")
                    {
                        Label = "1",
                        Data = new List<string>()
                        {
                            "True"
                        }
                    },
                    new Clause(Operation.IsNull, "IsExecutable")
                    {
                        Label = "2"
                    },
                    new Clause(Operation.IsNull, "IsExecutable")
                    {
                        Label = "3"
                    },
                    new Clause(Operation.IsNull, "IsExecutable")
                    {
                        Label = "4"
                    },
                    new Clause(Operation.IsNull, "IsExecutable")
                    {
                        Label = "5"
                    },
                    new Clause(Operation.IsNull, "IsExecutable")
                    {
                        Label = "6"
                    }
                }
            };

            Assert.IsTrue(analyzer.IsRuleValid(validRule));

            validRule = new Rule("StringsForClauseLabels")
            {
                Expression = "FOO AND BAR OR BA$_*",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "IsExecutable")
                    {
                        Label = "FOO"
                    },
                    new Clause(Operation.IsNull, "IsExecutable")
                    {
                        Label = "BAR"
                    },
                    new Clause(Operation.IsNull, "IsExecutable")
                    {
                        Label = "BA$_*"
                    }
                }
            };

            Assert.IsTrue(analyzer.IsRuleValid(validRule));

            validRule = new Rule("MultipleConsecutiveNots")
            {
                Expression = "0 AND NOT NOT 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "1"
                    },
                    new Clause(Operation.IsNull, "Path")
                    {
                        Label = "0"
                    }
                }
            };

            Assert.IsTrue(analyzer.IsRuleValid(validRule));
        }

        [TestMethod]
        public void VerifyBareObjectQuery()
        {
            var RuleName = "BareObjectRule";
            var bareObjectRule = new Rule(RuleName)
            {
                Target = "string",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq)
                    {
                        Data = new List<string>()
                        {
                            "MagicWord"
                        }
                    }
                }
            };

            var bareObjectRuleNoTarget = new Rule(RuleName)
            {
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq)
                    {
                        Data = new List<string>()
                        {
                            "MagicWord"
                        }
                    }
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { bareObjectRule, bareObjectRuleNoTarget };

            Assert.IsTrue(analyzer.Analyze(ruleList, "MagicWord").Count() == 2);
        }

        [TestMethod]
        public void VerifyXor()
        {
            var RuleName = "XorRule";
            var xorRule = new Rule(RuleName)
            {
                Expression = "0 XOR 1",
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Eq, "StringField")
                    {
                        Label = "0",
                        Data = new List<string>()
                        {
                            "MagicWord"
                        }
                    },
                    new Clause(Operation.IsTrue, "BoolField")
                    {
                        Label = "1"
                    }
                }
            };

            var analyzer = new Analyzer();
            var ruleList = new List<Rule>() { xorRule };

            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectTrueFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectTrueTrue).Any(x => x.Name == RuleName));
            Assert.IsTrue(!analyzer.Analyze(ruleList, testObjectFalseFalse).Any(x => x.Name == RuleName));
            Assert.IsTrue(analyzer.Analyze(ruleList, testObjectFalseTrue).Any(x => x.Name == RuleName));
        }

        [TestMethod]
        public void VerifyCustomRuleValidation()
        {
            var RuleName = "CustomRuleValidation";
            var supportedCustomOperation = new Rule(RuleName)
            {
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Custom, "Path")
                    {
                        CustomOperation = "FOO",
                        Data = new List<string>()
                        {
                            "Some Data"
                        }
                    },
                }
            };

            var unsupportedCustomOperation = new Rule(RuleName)
            {
                Target = "TestObject",
                Clauses = new List<Clause>()
                {
                    new Clause(Operation.Custom, "Path")
                    {
                        CustomOperation = "BAR",
                        Data = new List<string>()
                        {
                            "Some Data"
                        }
                    },
                }
            };


            var analyzer = new Analyzer();

            // Rules aren't valid without a validation delegate
            Assert.IsFalse(analyzer.IsRuleValid(supportedCustomOperation));

            analyzer.CustomOperationValidationDelegates.Add(parseFooOperations);

            (bool Applies, IEnumerable<Violation> FoundViolations) parseFooOperations(Rule r, Clause c)
            {
                switch (c.CustomOperation)
                {
                    case "FOO":
                        var violations = new List<Violation>();

                        if (!c.Data.Any())
                        {
                            violations.Add(new Violation("FOO Operation expects data", r, c));
                        }

                        return (true, violations);
                    default:
                        return (false, Array.Empty<Violation>());
                }
            };

            Assert.IsTrue(analyzer.IsRuleValid(supportedCustomOperation));
            Assert.IsFalse(analyzer.IsRuleValid(unsupportedCustomOperation));
        }
    }
}