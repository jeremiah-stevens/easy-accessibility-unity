using System.Collections.Generic;
using System.Reflection;
using EasyAccessibility.DescriptiveMedia;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests.DescriptiveMedia
{
    public class DescriptiveAudioClipSOTests
    {
        DescriptiveAudioClipSO m_so;
        List<Object> m_cleanup;

        static readonly FieldInfo k_Clip = typeof(DescriptiveAudioClipSO).GetField(
            "_clip",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_Category = typeof(DescriptiveAudioClipSO).GetField(
            "_category",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_SourceType = typeof(DescriptiveMediaSO).GetField(
            "_sourceType",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_Description = typeof(DescriptiveMediaSO).GetField(
            "_description",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_VttTranscript = typeof(DescriptiveMediaSO).GetField(
            "_vttTranscript",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        [SetUp]
        public void SetUp()
        {
            m_cleanup = new List<Object>();
            m_so = ScriptableObject.CreateInstance<DescriptiveAudioClipSO>();
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

        AudioClip MakeClip() =>
            Track(AudioClip.Create("test", 44100, 1, 44100, false));

        #region Defaults

        [Test]
        public void Clip_DefaultsToNull() => Assert.IsNull(m_so.Clip);

        [Test]
        public void Category_DefaultsToNone() =>
            Assert.AreEqual(AudioCategory.None, m_so.Category);

        [Test]
        public void VttTranscript_DefaultsToNull() => Assert.IsNull(m_so.VttTranscript);

        [Test]
        public void Duration_WhenClipIsNull_ReturnsZero() =>
            Assert.AreEqual(0f, m_so.Duration);

        [Test]
        public void MediaAsset_WhenClipIsNull_ReturnsNull() => Assert.IsNull(m_so.MediaAsset);

        #endregion

        #region Property accessors

        [Test]
        public void Clip_ReturnsSetValue()
        {
            var clip = MakeClip();
            k_Clip.SetValue(m_so, clip);
            Assert.AreSame(clip, m_so.Clip);
        }

        [Test]
        public void Asset_MatchesClip()
        {
            var clip = MakeClip();
            k_Clip.SetValue(m_so, clip);
            Assert.AreSame(clip, m_so.Asset);
        }

        [Test]
        public void MediaAsset_MatchesClip()
        {
            var clip = MakeClip();
            k_Clip.SetValue(m_so, clip);
            Assert.AreSame(clip, m_so.MediaAsset);
        }

        [Test]
        public void Transcript_MatchesVttTranscript()
        {
            var ta = Track(new TextAsset("WEBVTT"));
            k_VttTranscript.SetValue(m_so, ta);
            Assert.AreSame(ta, m_so.Transcript);
        }

        [Test]
        public void Duration_MatchesClipLength()
        {
            var clip = MakeClip();
            k_Clip.SetValue(m_so, clip);
            Assert.AreEqual(clip.length, m_so.Duration);
        }

        [Test]
        public void Category_ReturnsSetValue()
        {
            k_Category.SetValue(m_so, AudioCategory.Dialogue);
            Assert.AreEqual(AudioCategory.Dialogue, m_so.Category);
        }

        #endregion

        #region GetDescription

        [Test]
        public void GetDescription_WhenSourceTypeIsString_ReturnsDescription()
        {
            k_SourceType.SetValue(m_so, DescriptionSourceType.String);
            k_Description.SetValue(m_so, "A footstep on gravel.");
            Assert.AreEqual("A footstep on gravel.", m_so.GetDescription());
        }

        [Test]
        public void GetDescription_WhenSourceTypeIsVTT_ReturnsEmpty()
        {
            k_SourceType.SetValue(m_so, DescriptionSourceType.VTT);
            Assert.AreEqual(string.Empty, m_so.GetDescription());
        }

        #endregion

        #region Interface

        [Test]
        public void ImplementsITranscribedAsset() =>
            Assert.IsInstanceOf<ITranscribedAsset<AudioClip>>(m_so);

        #endregion
    }
}