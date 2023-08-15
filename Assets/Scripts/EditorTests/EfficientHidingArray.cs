using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace NL4_DataStructure
{
    public class EfficientHidingArray
    {
        [Test]
        public void Constructor_InitializesCorrectly()
        {
            int[] testData = { 1, 2, 3, 4, 5 };

            var hidingArray = new EfficientHidingArray<int>(testData);

            Assert.That(testData.Length,Is.EqualTo(hidingArray.Length));
        }

        [Test]
        public void Indexer_ReturnsCorrectElement()
        {
            int[] testData = { 1, 2, 3, 4, 5 };
            var hidingArray = new EfficientHidingArray<int>(testData);

            for (int i = 0; i < testData.Length; i++)
            {
                Assert.That(testData[i], Is.EqualTo(hidingArray[i]));
            }
        }
        [Test]
        public void Hide()
        {
            int[] testData = GenerateArray.Random(-99, 99, 10, 30);
            var hidingArray = new EfficientHidingArray<int>(testData);
            int[] randomHideIndex = GenerateArray.Random(0, testData.Length, 1, testData.Length);

            for (int j = 0; j < randomHideIndex.Length; j++)
            {
                hidingArray.ReserveHideAtIndex(randomHideIndex[j]);
            }
            hidingArray.Hide();

            Debug.Log("TestData");
            for (int j = 0; j < testData.Length; j++)
            {
                Debug.Log(testData[j]);
            }
            Debug.Log("RandomHideIndex");
            for (int i = 0; i < randomHideIndex.Length; i++)
            {
                Debug.Log(randomHideIndex[i]);
            }
            Debug.Log("HidingArray");
            for (int j = 0; j < hidingArray.Length; j++)
            {
                Debug.Log(hidingArray[j]);
            }
        }
    }
}
