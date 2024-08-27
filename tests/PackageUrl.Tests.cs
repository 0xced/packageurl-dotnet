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
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using PackageUrl.Tests.TestAssets;
using Xunit;

namespace PackageUrl.Tests;

/// <summary>
/// Test cases for PackageURL parsing.
/// </summary>
/// <remarks>
/// Original test cases retrieved from: https://raw.githubusercontent.com/package-url/purl-spec/master/test-suite-data.json
/// </remarks>
[SuppressMessage("Usage", "xUnit1045:Avoid using TheoryData type arguments that might not be serializable", Justification = "PurlTestData is serializable")]
public class PackageURLTest
{
    [Theory]
    [ClassData(typeof(PurlValidTheoryData))]
    public void TestValidConstructorParsing(PurlTestData data)
    {
        PackageURL purl = new PackageURL(data.Purl);

        purl.Should().BeEquivalentTo(data, options => options.ExcludingMissingMembers());
    }

    [Theory]
    [ClassData(typeof(PurlValidTheoryData))]
    public void TestValidConstructorParameters(PurlTestData data)
    {
        PackageURL purl = new PackageURL(data.Type, data.Namespace, data.Name, data.Version, data.Qualifiers, data.Subpath);

        purl.Should().BeEquivalentTo(data, options => options.ExcludingMissingMembers());
    }

    [Theory]
    [ClassData(typeof(PurlValidTheoryData))]
    public void TestSerialization(PurlTestData data)
    {
        PackageURL purl = new PackageURL(data.Purl);

        purl.ToString().Should().Be(data.CanonicalPurl);
    }

    [Theory]
    [ClassData(typeof(PurlInvalidTheoryData))]
    public void TestInvalidConstructorParsing(PurlTestData data)
    {
        Action act = () => _ = new PackageURL(data.Purl);

        act.Should().Throw<MalformedPackageUrlException>(because: data.Description);
    }

    [Theory]
    [ClassData(typeof(PurlInvalidTheoryData))]
    public void TestInvalidConstructorParameters(PurlTestData data)
    {
        Action act = () => _ = new PackageURL(data.Type, data.Namespace, data.Name, data.Version, data.Qualifiers, data.Subpath);

        act.Should().Throw<MalformedPackageUrlException>(because: data.Description);
    }
}