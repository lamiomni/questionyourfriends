﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Facebook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    My Friend's questions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%
        var questions = (List<QuestionYourFriendsDataAccess.Question>) ViewData["questions"];
        var friends = (Dictionary<long, JsonObject>) ViewData["friends"];
        if (questions == null || questions.Count == 0)
        {
        %>
        <div class="fbinfobox">  
            Your friends did not receive any question, go ask them some <%=Html.ActionLink("here", "Index", "Ask")%>!
        </div>
   <%
}
        else
        {
            foreach (var question in questions)
            {%>
       <div class="question-bloc">
       <%
                if (question.anom_price > 0)
                {%>
		    <img src="http://lamiomni.com/QuestionYourFriends/Content/annon.jpg" height="52" width="52" alt=""/>
	    <%
                }
                else
                {%>
		    <img src="http://graph.facebook.com/<%=question.Owner.fid%>/picture" height="52" width="52" alt=""/>
	    <%
                }%>
        <div class="question">
            <div class="question-status">
            <%
                if (question.anom_price > 0)
                {
%>
            <form method="post" action="FriendsQuestions/Reveal">
                <input type="hidden" name="qid" value="<%=question.id%>" />
                <button class="friendAnonymous" type="submit">???</button>
            </form>
        <%
                }
                else
                {
%>
                <span class="name">
                <%=friends[question.Owner.fid]["name"] %>
                </span>
        <%
                }
%>
        asked <span class="name"><%=friends[question.Receiver.fid]["name"]%></span>

            <div class="question-sentence"><%=question.text%></div>
            <%
                if (question.answer != null && question.date_answer.HasValue)
                {%>
		    <div class="answer-bloc">
			<img src="http://graph.facebook.com/<%=question.Receiver.fid%>/picture" height="33" width="33" alt=""/>
			<div class="answer">
				<div class="answer-status"><%=friends[question.Receiver.fid]["name"]%> answered:</div>
				<%=question.answer%> <br/> <%=question.date_answer.Value%>
			</div>
			</div>
            <%
                }%>
		</div>
        </div>
    </div>
    <%
            }
        }
%>
</asp:Content>
