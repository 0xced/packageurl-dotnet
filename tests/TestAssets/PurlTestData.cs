// MIT License
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace PackageUrl.Tests.TestAssets;

public record PurlTestData(
    string Description,
    string Purl,
    string CanonicalPurl,
    string Type,
    string Namespace,
    string Name,
    string Version,
    SortedDictionary<string, string> Qualifiers,
    string Subpath,
    bool IsInvalid)
{
    public override string ToString() => Purl;
}

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(PurlTestData[]))]
public partial class PurlTestSerializerContext : JsonSerializerContext;

public abstract class PurlTheoryData : TheoryData<PurlTestData>
{
    private readonly Lazy<PurlTestData[]> _testData = new(() =>
    {
        using var testSuiteStream = GetTestSuiteFile().OpenRead();
        return JsonSerializer.Deserialize(testSuiteStream, PurlTestSerializerContext.Default.PurlTestDataArray);
    });

    protected PurlTheoryData() => AddRange(_testData.Value.Where(Filter).ToArray());

    protected abstract bool Filter(PurlTestData data);

    private static FileInfo GetTestSuiteFile([CallerFilePath] string path = "") => new(Path.Combine(Path.GetDirectoryName(path)!, "test-suite-data.json"));
}

public class PurlValidTheoryData : PurlTheoryData
{
    protected override bool Filter(PurlTestData data) => !data.IsInvalid;
}

public class PurlInvalidTheoryData : PurlTheoryData
{
    protected override bool Filter(PurlTestData data) => data.IsInvalid;
}
