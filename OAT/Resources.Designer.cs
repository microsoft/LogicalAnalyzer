﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.CST.OAT {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.CST.OAT.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} contains both Data and DictData which is not allowed..
        /// </summary>
        internal static string Err_ClauseBothDataDictData {
            get {
                return ResourceManager.GetString("Err_ClauseBothDataDictData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} contains invalid characters after a close parenthesis {2}.
        /// </summary>
        internal static string Err_ClauseCharactersAfterClosedParentheses {
            get {
                return ResourceManager.GetString("Err_ClauseCharactersAfterClosedParentheses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} contains invalid characters before open parenthesis in label {2}.
        /// </summary>
        internal static string Err_ClauseCharactersBeforeOpenParentheses {
            get {
                return ResourceManager.GetString("Err_ClauseCharactersBeforeOpenParentheses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} contains invalid characters between close parentheses in label {2}.
        /// </summary>
        internal static string Err_ClauseCharactersBetweenClosedParentheses {
            get {
                return ResourceManager.GetString("Err_ClauseCharactersBetweenClosedParentheses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} contains invalid characters between open parentheses in label {2}.
        /// </summary>
        internal static string Err_ClauseCharactersBetweenOpenParentheses {
            get {
                return ResourceManager.GetString("Err_ClauseCharactersBetweenOpenParentheses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} contains close parenthesis in NOT operator {2}.
        /// </summary>
        internal static string Err_ClauseCloseParenthesesInNot {
            get {
                return ResourceManager.GetString("Err_ClauseCloseParenthesesInNot", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} contains DictData which is innapropriate for operation {2}..
        /// </summary>
        internal static string Err_ClauseDictDataUnexpected {
            get {
                return ResourceManager.GetString("Err_ClauseDictDataUnexpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} has clauses with duplicate name {1}.&quot;).
        /// </summary>
        internal static string Err_ClauseDuplicateName {
            get {
                return ResourceManager.GetString("Err_ClauseDuplicateName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} ends with an operator..
        /// </summary>
        internal static string Err_ClauseEndsWithOperator {
            get {
                return ResourceManager.GetString("Err_ClauseEndsWithOperator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} does not contain exactly one DateTime in its Data field..
        /// </summary>
        internal static string Err_ClauseExpectedDateTime {
            get {
                return ResourceManager.GetString("Err_ClauseExpectedDateTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} does not contain exactly one integer in its Data field..
        /// </summary>
        internal static string Err_ClauseExpectedInt {
            get {
                return ResourceManager.GetString("Err_ClauseExpectedInt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In rule {0} expression is not null but some clauses have no label..
        /// </summary>
        internal static string Err_ClauseExpressionButMissingLabels {
            get {
                return ResourceManager.GetString("Err_ClauseExpressionButMissingLabels", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} has clauses invalid name {1}. Names may not contains spaces or parentheses..
        /// </summary>
        internal static string Err_ClauseInvalidLabel {
            get {
                return ResourceManager.GetString("Err_ClauseInvalidLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} has Invalid Lambda (Message: {2})..
        /// </summary>
        internal static string Err_ClauseInvalidLambda__0__1__2_ {
            get {
                return ResourceManager.GetString("Err_ClauseInvalidLambda_{0}{1}{2}", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} contains NOT where a non-NOT operator was expected..
        /// </summary>
        internal static string Err_ClauseInvalidNotOperator {
            get {
                return ResourceManager.GetString("Err_ClauseInvalidNotOperator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} contains invalid boolean operator {2}.
        /// </summary>
        internal static string Err_ClauseInvalidOperator {
            get {
                return ResourceManager.GetString("Err_ClauseInvalidOperator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} contains invalid Regex {2}..
        /// </summary>
        internal static string Err_ClauseInvalidRegex {
            get {
                return ResourceManager.GetString("Err_ClauseInvalidRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} has specified custom operation but the CustomOperation field is missing..
        /// </summary>
        internal static string Err_ClauseMissingCustomOperation {
            get {
                return ResourceManager.GetString("Err_ClauseMissingCustomOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In rule {0} if any clause has labels they all must have labels..
        /// </summary>
        internal static string Err_ClauseMissingLabels {
            get {
                return ResourceManager.GetString("Err_ClauseMissingLabels", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} is missing Data List..
        /// </summary>
        internal static string Err_ClauseMissingListData {
            get {
                return ResourceManager.GetString("Err_ClauseMissingListData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operation {0} in Clause {2} of Rule {1} was not validated by any of the available custom validation delegates..
        /// </summary>
        internal static string Err_ClauseMissingValidationForOperation {
            get {
                return ResourceManager.GetString("Err_ClauseMissingValidationForOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} contains multiple NOTs in a row..
        /// </summary>
        internal static string Err_ClauseMultipleConsecutiveNots {
            get {
                return ResourceManager.GetString("Err_ClauseMultipleConsecutiveNots", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} does not contain any Data and will always return false..
        /// </summary>
        internal static string Err_ClauseNoData {
            get {
                return ResourceManager.GetString("Err_ClauseNoData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} does not contain any Data or DictData and will always return false..
        /// </summary>
        internal static string Err_ClauseNoDataOrDictData {
            get {
                return ResourceManager.GetString("Err_ClauseNoDataOrDictData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} contains invalid parenthesis in label {2}.
        /// </summary>
        internal static string Err_ClauseParenthesisInLabel {
            get {
                return ResourceManager.GetString("Err_ClauseParenthesisInLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} contains redundant Data field which will be ignored..
        /// </summary>
        internal static string Err_ClauseRedundantData {
            get {
                return ResourceManager.GetString("Err_ClauseRedundantData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} contains redundant DictData field which will be ignored..
        /// </summary>
        internal static string Err_ClauseRedundantDictData {
            get {
                return ResourceManager.GetString("Err_ClauseRedundantDictData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} has unbalanced parentheses..
        /// </summary>
        internal static string Err_ClauseUnbalancedParentheses {
            get {
                return ResourceManager.GetString("Err_ClauseUnbalancedParentheses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expression {0} in rule {1} contains undefined label {2}.
        /// </summary>
        internal static string Err_ClauseUndefinedLabel {
            get {
                return ResourceManager.GetString("Err_ClauseUndefinedLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} contains unexpected Dictionary Data..
        /// </summary>
        internal static string Err_ClauseUnexpectedDictData {
            get {
                return ResourceManager.GetString("Err_ClauseUnexpectedDictData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} uses unsupported Operator {2}:{3}..
        /// </summary>
        internal static string Err_ClauseUnsuppportedOperator__0__1__2__3_ {
            get {
                return ResourceManager.GetString("Err_ClauseUnsuppportedOperator_{0}{1}{2}{3}", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Clause {0} is declared but never used in rule {1}..
        /// </summary>
        internal static string Err_ClauseUnusedLabel {
            get {
                return ResourceManager.GetString("Err_ClauseUnusedLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule {0} Clause {1} has Operation {2} with undefined validation delegate..
        /// </summary>
        internal static string Err_ValidationDelegateUndefined__0__1__2_ {
            get {
                return ResourceManager.GetString("Err_ValidationDelegateUndefined_{0}{1}{2}", resourceCulture);
            }
        }
    }
}
