using System.Linq;
using AngleSharp.Css.Dom;
using AngleSharp.Dom;
using BlazorTimeline;
using Bunit;
using FluentAssertions;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace Tests {
	public class TimelineTests {
		private TestContext _context;

		[SetUp]
		public void Setup() {
			_context = new TestContext();
		}

		[Test]
		public void Timeline_NoParams_ShouldRender() {
			// Arrange
			// Act
			var timeline = _context.RenderComponent<Timeline>();
			// Assert
			var container = timeline.Find("div");
			var projectTitle = container.GetElementsByTagName("h1");
			projectTitle.Should().HaveCount(1);
			projectTitle[0].ClassName.Should().Be("project-name");
			projectTitle[0].Text().Should().BeNullOrEmpty();

			var divs = container.GetElementsByTagName("div");
			divs.Should().HaveCount(1);
			var div = divs[0];
			var styles = div.GetStyle().ToList();
			styles.Should().HaveCount(4);
			styles[0].Name.Should().Be("--title-bg");
			styles[0].Value.Should().Be(timeline.Instance.TitleBgColor);
			styles[1].Name.Should().Be("--title-color");
			styles[1].Value.Should().Be(timeline.Instance.TitleColor);
			styles[2].Name.Should().Be("--text-color");
			styles[2].Value.Should().Be(timeline.Instance.TextColor);
			styles[3].Name.Should().Be("--text-bg");
			styles[3].Value.Should().Be(timeline.Instance.TextBgColor);
		}
		
		[Test]
		public void Timeline_Title_ShouldRenderWithTitle() {
			// Arrange
			const string expectedTitle = "Test Title";
			// Act
			var timeline = _context.RenderComponent<Timeline>(
				ComponentParameter.CreateParameter(nameof(Timeline.Title), expectedTitle)
				);
			// Assert
			var container = timeline.Find("div");
			var projectTitle = container.GetElementsByTagName("h1");
			projectTitle.Should().HaveCount(1);
			projectTitle[0].Text().Should().Be(expectedTitle);
		}
		
		[Test]
		public void Timeline_TitleBgColor_ShouldCreateCssVariable() {
			// Arrange
			const string expected = "#000000";
			// Act
			var timeline = _context.RenderComponent<Timeline>(
				ComponentParameter.CreateParameter(nameof(Timeline.TitleBgColor), expected)
			);
			// Assert
			var container = timeline.Find("div");
			var divs = container.GetElementsByTagName("div");
			divs.Should().HaveCount(1);
			var div = divs[0];
			var styles = div.GetStyle().ToList();
			styles.Should().HaveCount(4);
			var cssVariable = styles.Single(a => a.Name == "--title-bg");
			cssVariable.Value.Should().Be(expected);
		}
		
		[Test]
		public void Timeline_TitleColor_ShouldCreateCssVariable() {
			// Arrange
			const string expected = "#000000";
			// Act
			var timeline = _context.RenderComponent<Timeline>(
				ComponentParameter.CreateParameter(nameof(Timeline.TitleColor), expected)
			);
			// Assert
			var container = timeline.Find("div");
			var divs = container.GetElementsByTagName("div");
			divs.Should().HaveCount(1);
			var div = divs[0];
			var styles = div.GetStyle().ToList();
			styles.Should().HaveCount(4);
			var cssVariable = styles.Single(a => a.Name == "--title-color");
			cssVariable.Value.Should().Be(expected);
		}
		
		[Test]
		public void Timeline_TextBgColor_ShouldCreateCssVariable() {
			// Arrange
			const string expected = "#000000";
			// Act
			var timeline = _context.RenderComponent<Timeline>(
				ComponentParameter.CreateParameter(nameof(Timeline.TextBgColor), expected)
			);
			// Assert
			var container = timeline.Find("div");
			var divs = container.GetElementsByTagName("div");
			divs.Should().HaveCount(1);
			var div = divs[0];
			var styles = div.GetStyle().ToList();
			styles.Should().HaveCount(4);
			var cssVariable = styles.Single(a => a.Name == "--text-bg");
			cssVariable.Value.Should().Be(expected);
		}
		
		[Test]
		public void Timeline_TextColor_ShouldCreateCssVariable() {
			// Arrange
			const string expected = "#000000";
			// Act
			var timeline = _context.RenderComponent<Timeline>(
				ComponentParameter.CreateParameter(nameof(Timeline.TextColor), expected)
			);
			// Assert
			var container = timeline.Find("div");
			var divs = container.GetElementsByTagName("div");
			divs.Should().HaveCount(1);
			var div = divs[0];
			var styles = div.GetStyle().ToList();
			styles.Should().HaveCount(4);
			var cssVariable = styles.Single(a => a.Name == "--text-color");
			cssVariable.Value.Should().Be(expected);
		}
		
		[Test]
		[TestCase(ItemPositionOption.Manual)]
		[TestCase(ItemPositionOption.AutoAltering)]
		public void Timeline_ItemPositionOption_ShouldSetItemPositionOption(ItemPositionOption option) {
			// Arrange
			// Act
			var timeline = _context.RenderComponent<Timeline>(
				ComponentParameter.CreateParameter(nameof(Timeline.ItemPositionOption), option)
			);
			// Assert
			timeline.Instance.ItemPositionOption.Should().Be(option);
		}
		
			[Test]
		public void Timeline_DefaultItemPositionWithoutPosition_ShouldNotHavePositionRight() {
			// Arrange
			// Act
			var component = _context.RenderComponent<Timeline>(p =>
				p.AddChildContent<TimelineItem>());
			var timelineItem = component.FindComponent<TimelineItem>();
			var timelineContent = timelineItem.Find("div").GetElementsByClassName("timeline-content")[0];
			// Assert
			timelineContent.ClassList.Should().NotContain(a => a == "right");
		}

		[Test]
		public void Timeline_DefaultItemPositionTwoWithoutPosition_ShouldSecondHavePositionRight() {
			// Arrange
			// Act
			var component = _context.RenderComponent<Timeline>(p => {
				p.AddChildContent<TimelineItem>();
				p.AddChildContent<TimelineItem>();
			});
			var timelineItems = component.FindComponents<TimelineItem>();
			timelineItems.Should().HaveCount(2);
			var secondTimelineItem = timelineItems[1];
			var timelineContent = secondTimelineItem.Find("div").GetElementsByClassName("timeline-content")[0];
			// Assert
			timelineContent.ClassList.Should().Contain(a => a == "right");
		}

		[Test]
		public void Timeline_DefaultItemPositionTwoWithPosition_ShouldSecondHavePositionRight() {
			// Arrange
			// Act
			var component = _context.RenderComponent<Timeline>(p => {
				p.AddChildContent<TimelineItem>();
				p.AddChildContent<TimelineItem>(a => a.Add(a => a.Position, ItemPosition.Left));
			});
			var timelineItems = component.FindComponents<TimelineItem>();
			timelineItems.Should().HaveCount(2);
			var secondTimelineItem = timelineItems[1];
			var timelineContent = secondTimelineItem.Find("div").GetElementsByClassName("timeline-content")[0];
			// Assert
			timelineContent.ClassList.Should().Contain(a => a == "right");
		}

		[Test]
		public void Timeline_ManualItemPositionTwoWithPosition_ShouldSecondHavePositionRight() {
			// Arrange
			// Act
			var component = _context.RenderComponent<Timeline>(p => {
				p.Add(a => a.ItemPositionOption, ItemPositionOption.Manual);
				p.AddChildContent<TimelineItem>(a => a.Add(a => a.Position, ItemPosition.Right));
				p.AddChildContent<TimelineItem>(a => a.Add(a => a.Position, ItemPosition.Left));
			});
			var timelineItems = component.FindComponents<TimelineItem>();
			timelineItems.Should().HaveCount(2);
			var firstTimelineContent = timelineItems[0].Find("div").GetElementsByClassName("timeline-content")[0];;
			var secondTimelineContent = timelineItems[1].Find("div").GetElementsByClassName("timeline-content")[0];;
			// Assert
			firstTimelineContent.ClassList.Should().Contain(a => a == "right");
			secondTimelineContent.ClassList.Should().NotContain(a => a == "right");
		}
	}
}