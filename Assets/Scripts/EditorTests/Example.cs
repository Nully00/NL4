using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests
{
    public class Example
    {
        // Testアトリビュートを付ける
        [Test]
        public void ExampleTest()
        {
            // 条件式がtrueだったら成功
            Assert.That(1 < 10);

            // 1個目の引数が2個目より小さければ成功
            Assert.That(10, Is.LessThan(100));

            // 1個目の引数が2個目の引数の範囲内なら成功
            Assert.That(10, Is.InRange(0, 100));

            // 1個めの引数の文字列が2個目の文字列で始まっていたら成功
            Assert.That("Example", Does.StartWith("Ex"));

            // 1個めの引数の文字列に2個目の文字列が含まれていたら成功
            Assert.That("Example", Does.Contain("xamp"));

            // 1個めの引数の文字列が2個目の文字列で終わっていたら成功
            Assert.That("Example", Does.EndWith("ple"));

            // 1個めの引数の文字列が2個目の正規表現にマッチしたら成功
            Assert.That("Example", Does.Match("Ex*"));
        }
        [Test]
        public void Test01()
        {
            var value = 0.00001f;
            var expected = 0f;
            var comparer = FloatEqualityComparer.Instance;

            Assert.That(value, Is.EqualTo(expected).Using(comparer));
        }
        // このクラスに定義された各テストが実行される前に、テストごとに一回ずつ呼ばれる
        [SetUp]
        public void Setup()
        {
            Debug.Log("Setup");
        }

        // このクラスに定義された各テストの実行終了後に、テストごとに一回ずつ呼ばれる
        [TearDown]
        public void TearDown()
        {
            Debug.Log("TearDown");
        }

        // このクラスに定義されたテストのうち最初のテストが実行される前に一回呼ばれる
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Debug.Log("OneTimeSetUp");
        }

        // このクラスに定義されたテストのうち最後のテストが実行された後に一回呼ばれる
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Debug.Log("OneTimeTearDown");
        }
    }
}
