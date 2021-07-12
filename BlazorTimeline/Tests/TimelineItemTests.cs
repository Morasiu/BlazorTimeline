using System;
using System.IO;
using System.Linq;
using System.Net;
using AngleSharp;
using AngleSharp.Dom;
using BlazorTimeline;
using Bunit;
using FluentAssertions;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace Tests {
	public class TimelineItemTests {
		private TestContext _context;

		[SetUp]
		public void Setup() {
			_context = new TestContext();
		}

		[Test]
		public void TimelineItem_WithoutTimeline_ShouldThrowArgumentNullException() {
			// Arrange
			// Act
			Action action = () => _context.RenderComponent<TimelineItem>();
			// Assert
			action.Should().ThrowExactly<ArgumentNullException>();
		}

		[Test]
		public void TimelineItem_WithTimeline_ShouldRender() {
			// Arrange
			// Act
			Action action = () => _context.RenderComponent<Timeline>(p =>
				p.AddChildContent<TimelineItem>());
			// Assert
			action.Should().NotThrow<ArgumentNullException>();
		}

		[Test]
		public void TimelineItem_WithTimeline_ShouldRenderHaveTimelineSet() {
			// Arrange
			// Act
			const string expected = "Test title";
			var component = _context.RenderComponent<Timeline>(p =>
				p.AddChildContent<TimelineItem>(builder => builder.Add(a => a.Title, expected)));
			var timelineItem = component.FindComponent<TimelineItem>();
			// Assert
			timelineItem.Instance.Timeline.Should().Be(component.Instance);
		}

		[Test]
		public void TimelineItem_WithTitle_ShouldRenderTitle() {
			// Arrange
			// Act
			const string expected = "Test title";
			var component = _context.RenderComponent<Timeline>(p =>
				p.AddChildContent<TimelineItem>(builder => builder.Add(a => a.Title, expected)));
			var timelineItem = component.FindComponent<TimelineItem>();
			var divTitle = timelineItem.Find("div").GetElementsByClassName("item-title")[0];
			var title = divTitle.GetElementsByTagName("h2")[0];
			// Assert
			timelineItem.Instance.Title.Should().Be(expected);
			title.Text().Should().Be(expected);
		}
		
		[Test]
		public void TimelineItem_WithoutIconContent_ShouldRenderDefaultIcon() {
			// Arrange
			var iconPath = "_content/BlazorTimeline/default_icon.svg";
			// Act
			var component = _context.RenderComponent<Timeline>(p =>
				p.AddChildContent<TimelineItem>());
			var timelineItem = component.FindComponent<TimelineItem>();
			var iconDiv = timelineItem.Find("div").GetElementsByClassName("timeline-icon")[0];
			var icon = iconDiv.GetElementsByTagName("img")[0];
			// Assert
			timelineItem.Instance.IconContent.Should().BeNull();
			icon.GetAttribute("src").Should().Be(iconPath);
		}
		
		[Test]
		public void TimelineItem_WithIconContent_ShouldRenderCustomIcon() {
			// Arrange
			var customIcon = "<svg><circle cx=\"50\" cy=\"50\" r=\"40\" stroke=\"black\" stroke-width=\"3\" fill=\"red\"></circle></svg>";
			// Act
			var component = _context.RenderComponent<Timeline>(p =>
				p.AddChildContent<TimelineItem>(builder => builder.Add(a => a.IconContent, customIcon)));
			var timelineItem = component.FindComponent<TimelineItem>();
			var iconDiv = timelineItem.Find("div").GetElementsByClassName("timeline-icon")[0];
			// Assert
			timelineItem.Instance.IconContent.Should().NotBeNull();
			iconDiv.InnerHtml.Should().Be(customIcon);
		}

		[Test]
		public void TimelineItem_WithTime_ShouldRenderTime() {
			// Arrange
			// Act
			const string expected = "01.01.0001";
			var component = _context.RenderComponent<Timeline>(p =>
				p.AddChildContent<TimelineItem>(builder => builder.Add(a => a.Time, expected)));
			var timelineItem = component.FindComponent<TimelineItem>();
			var divTitle = timelineItem.Find("div").GetElementsByClassName("item-title")[0];
			var time = divTitle.GetElementsByTagName("span")[0].GetElementsByTagName("i")[0];
			// Assert
			timelineItem.Instance.Time.Should().Be(expected);
			time.Text().Should().Be(expected);
		}

		[Test]
		public void TimelineItem_WithoutButtonText_ShouldNotRenderButton() {
			// Arrange
			// Act
			var component = _context.RenderComponent<Timeline>(p =>
				p.AddChildContent<TimelineItem>());
			var timelineItem = component.FindComponent<TimelineItem>();
			var timelineContent = timelineItem.Find("div");
			var buttons = timelineContent.GetElementsByClassName("timeline-button");
			// Assert
			buttons.Should().HaveCount(0);
		}

		[Test]
		public void TimelineItem_WithButtonText_ShouldNotRenderButton() {
			// Arrange
			// Act
			var expected = "Test button text";
			var component = _context.RenderComponent<Timeline>(p =>
				p.AddChildContent<TimelineItem>(a => a.Add(t => t.ButtonText, expected)));
			var timelineItem = component.FindComponent<TimelineItem>();
			var timelineContent = timelineItem.Find("div").GetElementsByClassName("timeline-content")[0];
			var buttons = timelineContent.GetElementsByClassName("timeline-button");
			// Assert
			buttons.Should().HaveCount(1);
			buttons[0].Text().Should().Be(expected);
		}

		[Test]
		public void TimelineItem_WithButtonTextAndLink_ShouldNotRenderButtonWithLink() {
			// Arrange
			// Act
			var expected = "Test link";
			var component = _context.RenderComponent<Timeline>(p =>
				p.AddChildContent<TimelineItem>(a => {
					a.Add(t => t.ButtonText, "Test");
					a.Add(t => t.Link, expected);
				}));
			var timelineItem = component.FindComponent<TimelineItem>();
			var timelineContent = timelineItem.Find("div").GetElementsByClassName("timeline-content")[0];
			var buttons = timelineContent.GetElementsByClassName("timeline-button");
			// Assert
			buttons.Should().HaveCount(1);
			buttons[0].Attributes.Single(a => a.Name == "href").Value.Should().Be(expected);
		}
		
		[Test]
		public void TimelineItem_ChildContent_ShouldRenderChildContent() {
			// Arrange
			// Act
			var expected = "<span>Hello World!</span>";
			var component = _context.RenderComponent<Timeline>(p =>
				p.AddChildContent<TimelineItem>(a => {
					a.AddChildContent(expected);
				}));
			var timelineItem = component.FindComponent<TimelineItem>();
			var timelineContent = timelineItem.Find("div").GetElementsByClassName("timeline-content")[0];
			var childContent = timelineContent.GetElementsByTagName("p");
			// Assert
			childContent.Should().HaveCount(1);
			childContent[0].InnerHtml.Should().Be(expected);
		}
	}
}