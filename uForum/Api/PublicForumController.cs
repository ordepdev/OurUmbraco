﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using uForum.Services;
using Umbraco.Web.WebApi;

namespace uForum.Api
{
    public class PublicForumController : ForumControllerBase
    {

        /* TOPICS */
        [HttpGet]
        public IEnumerable<ExpandoObject> LatestPaged(int page, int cat)
        {
            var l = new List<ExpandoObject>();
            foreach (var topic in TopicService.GetLatestTopics(50, page, true, cat).Items)
            {
                dynamic o = new ExpandoObject();

                o.url = topic.GetUrl();
                o.title = topic.Title;
                o.replies = topic.Replies;
                o.hasAnswer = topic.Answer > 0;
                o.updated = topic.Updated.ConvertToRelativeTime();
                if (topic.LatestReplyAuthor > 0)
                {
                    var mem = Members.GetById(topic.LatestReplyAuthor);
                    if (mem != null)
                    {
                        o.memId = mem.Id;
                        o.memName = mem.Name;
                    }
                }
                else
                {
                    var author = Members.GetById(topic.MemberId);
                    if (author != null)
                    {
                        o.memId = author.Id;
                        o.memName = author.Name;
                    }
                }

                var forum = Umbraco.TypedContent(topic.ParentId);
                if (forum != null)
                {
                    o.forumUrl = forum.Url;
                    o.forumName = forum.Name;
                }

                l.Add(o);
            }
            return l;
        }

        [HttpGet]
        public string CategoryUrl(int id)
        {
            return Umbraco.TypedContent(id).Url;
        }
    }
}