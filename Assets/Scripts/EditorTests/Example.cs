using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests
{
    public class Example
    {
        // Test�A�g���r���[�g��t����
        [Test]
        public void ExampleTest()
        {
            // ��������true�������琬��
            Assert.That(1 < 10);

            // 1�ڂ̈�����2�ڂ�菬������ΐ���
            Assert.That(10, Is.LessThan(100));

            // 1�ڂ̈�����2�ڂ̈����͈͓̔��Ȃ琬��
            Assert.That(10, Is.InRange(0, 100));

            // 1�߂̈����̕�����2�ڂ̕�����Ŏn�܂��Ă����琬��
            Assert.That("Example", Does.StartWith("Ex"));

            // 1�߂̈����̕������2�ڂ̕����񂪊܂܂�Ă����琬��
            Assert.That("Example", Does.Contain("xamp"));

            // 1�߂̈����̕�����2�ڂ̕�����ŏI����Ă����琬��
            Assert.That("Example", Does.EndWith("ple"));

            // 1�߂̈����̕�����2�ڂ̐��K�\���Ƀ}�b�`�����琬��
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
        // ���̃N���X�ɒ�`���ꂽ�e�e�X�g�����s�����O�ɁA�e�X�g���ƂɈ�񂸂Ă΂��
        [SetUp]
        public void Setup()
        {
            Debug.Log("Setup");
        }

        // ���̃N���X�ɒ�`���ꂽ�e�e�X�g�̎��s�I����ɁA�e�X�g���ƂɈ�񂸂Ă΂��
        [TearDown]
        public void TearDown()
        {
            Debug.Log("TearDown");
        }

        // ���̃N���X�ɒ�`���ꂽ�e�X�g�̂����ŏ��̃e�X�g�����s�����O�Ɉ��Ă΂��
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Debug.Log("OneTimeSetUp");
        }

        // ���̃N���X�ɒ�`���ꂽ�e�X�g�̂����Ō�̃e�X�g�����s���ꂽ��Ɉ��Ă΂��
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Debug.Log("OneTimeTearDown");
        }
    }
}
