using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests.Transcript
{
    public class AssetTranscriptSOTests
    {
        AssetTranscriptSO m_so;
        List<Object> m_cleanup;

        static readonly FieldInfo k_Asset = typeof(AssetTranscriptSO).GetField(
            "asset",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_Transcript = typeof(AssetTranscriptSO).GetField(
            "transcript",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        [SetUp]
        public void SetUp()
        {
            m_cleanup = new List<Object>();
            m_so = ScriptableObject.CreateInstance<AssetTranscriptSO>();
            m_cleanup.Add(m_so);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var obj in m_cleanup)
                if (obj != null)
                    DestroyImmediate(obj);
        }

        T Track<T>(T obj)
            where T : Object
        {
            m_cleanup.Add(obj);
            return obj;
        }

        #region Defaults

        [Test]
        public void Asset_DefaultsToNull()
        {
            Assert.IsNull(m_so.Asset);
        }

        [Test]
        public void Transcript_DefaultsToNull()
        {
            Assert.IsNull(m_so.Transcript);
        }

        #endregion

        #region Property accessors

        [Test]
        public void Asset_ReturnsSetValue()
        {
            var tex = Track(new Texture2D(1, 1));
            k_Asset.SetValue(m_so, tex);
            Assert.AreSame(tex, m_so.Asset);
        }

        [Test]
        public void Transcript_ReturnsSetValue()
        {
            var ta = Track(new TextAsset("test content"));
            k_Transcript.SetValue(m_so, ta);
            Assert.AreSame(ta, m_so.Transcript);
        }

        #endregion

        #region Interface

        [Test]
        public void ImplementsIAssetTranscript()
        {
            Assert.IsInstanceOf<IAssetTranscript<Object>>(m_so);
        }

        #endregion
    }
}
