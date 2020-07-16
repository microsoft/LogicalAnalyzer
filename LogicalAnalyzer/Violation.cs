﻿// Copyright (c) Microsoft Corporation. All rights reserved. Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Microsoft.CST.LogicalAnalyzer
{
    public class Violation
    {
        /// <summary>
        ///     The Rule associated with this violation
        /// </summary>
        public Rule rule { get; set; }
        /// <summary>
        ///     The clauses associated with this violation
        /// </summary>
        public Clause[] clauses { get; set; }
        /// <summary>
        ///     The text description of the violation
        /// </summary>
        public string description { get; set; }

        public Violation(string description, Rule rule) : this(description, rule, Array.Empty<Clause>()) { }
        public Violation(string description, Rule rule, Clause clause) : this(description, rule, new Clause[] { clause }) { }
        public Violation(string description, Rule rule, Clause[] clauses)
        {
            this.description = description ?? throw new ArgumentNullException(nameof(description), $"{nameof(description)} may not be null");
            this.rule = rule ?? throw new ArgumentNullException(nameof(rule), $"{nameof(rule)} may not be null");
            this.clauses = clauses ?? throw new ArgumentNullException(nameof(clauses), $"{nameof(clauses)} may not be null");
        }
    }
}