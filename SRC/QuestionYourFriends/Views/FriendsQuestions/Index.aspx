<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    My Friend's questions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%:ViewData["Message"]%></h2>
    <%
        var questions = (List<QuestionYourFriendsDataAccess.Question>) ViewData["questions"];
        var friends = (Dictionary<long, dynamic>) ViewData["friends"];
        foreach (var question in questions)
        {
            if(!friends.ContainsKey(question.Owner.fid))
                continue;
%>
       <div class="question-bloc">
       <% if (question.anom_price > 0) {%>
		    <img src="Content/annon.jpg" height="52" width="52" alt=""/>
	    <% } else { %>
		    <img src="http://graph.facebook.com/<%:question.Owner.fid %>/picture" height="52" width="52" alt=""/>
	    <%} %>
        <div class="question">
            <div class="question-status">
            <%
              if (question.anom_price == 0)
              {
            %>
                <span class="name">
                <%:friends[question.Owner.fid].name%>
                </span>
        <%
               }
               else
               {
        %>
            <form method="post" action="FriendsQuestions/Reveal">
                <input type="hidden" name="qid" value="<%:question.id%>" />
                <button type="submit">???</button>
            </form>
        <%
                }
        %>
        asked <span class="name"><%:friends[question.Receiver.fid].name %></span>

            <div class="question-sentence"><%:question.text%></div>
            <% if (question.answer != null) {%>
		    <div class="answer-bloc">
			<img src="http://graph.facebook.com/<%:question.Receiver.fid %>/picture" height="33" width="33" alt=""/>
			<div class="answer">
				<div class="answer-status"><%:friends[question.Receiver.fid].name%> answered:</div>
				<%:question.answer%> <br/> <%:question.date_pub%>
			</div>
			</div>
            <% } %>
		</div>
        </div>
    </div>
    <%
    }
    %>
</asp:Content>
