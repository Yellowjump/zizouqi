﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.IO;

namespace DataTable.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class BoolProcessor : GenericDataProcessor<bool>
        {
            public override bool IsSystem
            {
                get
                {
                    return true;
                }
            }

            public override string LanguageKeyword
            {
                get
                {
                    return "bool";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "bool",
                    "boolean",
                    "system.boolean"
                };
            }

            public override bool Parse(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }
                if (int.TryParse(value, out var intResult))
                {
                    return intResult >= 1;
                }
                return bool.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(Parse(value));
            }
        }
    }
}