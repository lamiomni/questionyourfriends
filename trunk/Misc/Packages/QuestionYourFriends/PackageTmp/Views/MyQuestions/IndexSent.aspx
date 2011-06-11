<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Questions Sent
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%:ViewData["Message"]%></h2>
    <div style="width:100%;margin-bottom:40px;">
    </div>
    <ul id="boxes">
    <%
        var received = ((string) ViewData["tab"] == "toMe") ? "class=actif" : "";
        var sent = ((string) ViewData["tab"] == "fromMe") ? "class=actif" : "";
    %>
        <li <%:received %>><%:Html.ActionLink("Questions received", "Index", "MyQuestions")%></li>
        <li <%:sent %>><%:Html.ActionLink("Questions sent", "FromMe", "MyQuestions")%></li>
    </ul>
    <%
        var questions = (List<QuestionYourFriendsDataAccess.Question>) ViewData["questions"];
        var friends = (Dictionary<long, dynamic>) ViewData["friends"];
        if (questions == null || questions.Count == 0)
        {
        %>
        <div class="fbinfobox">  
            You did not send any question, go ask some to your friends!
        </div>
   <%
}
        else
        {
            foreach (var question in questions)
            {
%>
       <div class="question-bloc">
		<img src="http://graph.facebook.com/<%:question.Owner.fid%>/picture" height="52" width="52" alt=""/>
        <div class="question">
            <div class="question-status">
                <span class="name">
                You
                </span>
        asked <span class="name">
        <%
                if (!friends.ContainsKey(question.Receiver.fid))
                {
%>
            Qyf
        <%
                }
                else
                {%>
            <%:friends[question.Receiver.fid].name%><%
                }%>
        </span> a
        <%
                if (question.private_price > 0)
                {%>
            <span class="privacy">private</span>
        <%
                }
                else
                {%>
            <span class="privacy">public</span>
        <%
                }%>
            question.
            <div class="question-sentence"><%:question.text%></div>
            <%
                if (question.answer != null)
                {%>
		    <div class="answer-bloc">
			<img src="http://graph.facebook.com/<%:question.Receiver.fid%>/picture" height="33" width="33" alt=""/>
			<div class="answer">
				<div class="answer-status">
                <%
                    if (!friends.ContainsKey(question.Receiver.fid))
                    {
%>
                        Qyf
                    <%
                    }
                    else
                    {%>
                        <%:friends[question.Receiver.fid].name%> <%
                    }%> answered:</div>
				<%:question.answer%> <br/> <%:question.date_pub%>
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
