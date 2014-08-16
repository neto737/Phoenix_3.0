using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core;
using Phoenix.Messages;
using Phoenix.Storage;
namespace Phoenix.HabboHotel.Support
{
	class HelpTool
	{
        public Dictionary<uint, HelpCategory> Categories = new Dictionary<uint, HelpCategory>();
        public Dictionary<uint, HelpTopic> Topics = new Dictionary<uint, HelpTopic>();
        public List<HelpTopic> ImportantTopics = new List<HelpTopic>();
        public List<HelpTopic> KnownIssues = new List<HelpTopic>();

		public void LoadCategories(DatabaseClient dbClient)
		{
			Logging.Write("Loading Help Categories..");
			this.Categories.Clear();
			DataTable dataTable = dbClient.ReadDataTable("SELECT Id, caption FROM help_subjects");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					this.Categories.Add((uint)dataRow["Id"], new HelpCategory((uint)dataRow["Id"], (string)dataRow["caption"]));
				}
				Logging.WriteLine("completed!");
			}
		}

		public HelpCategory GetCategory(uint CategoryId)
		{
			if (this.Categories.ContainsKey(CategoryId))
			{
				return this.Categories[CategoryId];
			}
            return null;
		}

		public void ClearCategories()
		{
			this.Categories.Clear();
		}

		public void LoadTopics(DatabaseClient dbClient)
		{
			Logging.Write("Loading Help Topics..");
			this.Topics.Clear();
			DataTable dataTable = dbClient.ReadDataTable("SELECT Id, title, body, subject, known_issue FROM help_topics");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					HelpTopic topic = new HelpTopic((uint)dataRow["Id"], (string)dataRow["title"], (string)dataRow["body"], (uint)dataRow["subject"]);
					this.Topics.Add((uint)dataRow["Id"], topic);
					int num = int.Parse(dataRow["known_issue"].ToString());
					if (num == 1)
					{
						this.KnownIssues.Add(topic);
					}
					else if (num == 2)
					{
						this.ImportantTopics.Add(topic);
					}
				}
				Logging.WriteLine("completed!");
			}
		}

		public HelpTopic GetTopic(uint TopicId)
		{
			if (this.Topics.ContainsKey(TopicId))
			{
				return this.Topics[TopicId];
			}
            return null;
		}

		public void ClearTopics()
		{
			this.Topics.Clear();
			this.ImportantTopics.Clear();
			this.KnownIssues.Clear();
		}

		public int ArticlesInCategory(uint CategoryId)
		{
			int num = 0;
            foreach (HelpTopic topic in this.Topics.Values)
			{
				if (topic.CategoryId == CategoryId)
				{
					num++;
				}
			}
			return num;
		}

		public ServerMessage SerializeFrontpage()
		{
			ServerMessage Message = new ServerMessage(518);
			Message.AppendInt32(this.ImportantTopics.Count);
			foreach (HelpTopic topic in this.ImportantTopics)
			{
				Message.AppendUInt(topic.TopicId);
				Message.AppendStringWithBreak(topic.Caption);
			}
			Message.AppendInt32(this.KnownIssues.Count);
			foreach (HelpTopic topic2 in this.KnownIssues)
			{
				Message.AppendUInt(topic2.TopicId);
				Message.AppendStringWithBreak(topic2.Caption);
			}
			return Message;
		}
        
		public ServerMessage SerializeIndex()
		{
			ServerMessage message = new ServerMessage(519);
			message.AppendInt32(this.Categories.Count);
			foreach (HelpCategory current in this.Categories.Values)
			{
				message.AppendUInt(current.CategoryId);
				message.AppendStringWithBreak(current.Caption);
				message.AppendInt32(this.ArticlesInCategory(current.CategoryId));
			}
			return message;
		}

		public ServerMessage SerializeTopic(HelpTopic Topic)
		{
			ServerMessage Message = new ServerMessage(520);
			Message.AppendUInt(Topic.TopicId);
			Message.AppendStringWithBreak(Topic.Body);
			return Message;
		}

		public ServerMessage SerializeSearchResults(string Query)
		{
			DataTable table = null;
			using (DatabaseClient client = PhoenixEnvironment.GetDatabase().GetClient())
			{
				client.AddParamWithValue("query", Query);
				table = client.ReadDataTable("SELECT Id,title FROM help_topics WHERE title LIKE @query OR body LIKE @query LIMIT 25");
			}
			ServerMessage Message = new ServerMessage(521);
			if (table == null)
			{
				Message.AppendBoolean(false);
				return Message;
			}
			Message.AppendInt32(table.Rows.Count);
			foreach (DataRow dataRow in table.Rows)
			{
				Message.AppendUInt((uint)dataRow["Id"]);
				Message.AppendStringWithBreak((string)dataRow["title"]);
			}
			return Message;
		}

		public ServerMessage SerializeCategory(HelpCategory Category)
		{
			ServerMessage Message = new ServerMessage(522);
			Message.AppendUInt(Category.CategoryId);
			Message.AppendStringWithBreak("");
			Message.AppendInt32(this.ArticlesInCategory(Category.CategoryId));
			using (TimedLock.Lock(this.Topics))
			{
				foreach (HelpTopic topic in this.Topics.Values)
				{
					if (topic.CategoryId == Category.CategoryId)
					{
						Message.AppendUInt(topic.TopicId);
						Message.AppendStringWithBreak(topic.Caption);
					}
				}
			}
			return Message;
		}
	}
}
