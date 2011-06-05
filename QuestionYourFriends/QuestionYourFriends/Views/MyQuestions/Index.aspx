<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" CodeBehind="~/Controllers/MyQuestionsController.cs"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Mes questions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fb:serverFbml  width="300px" >
    <script type="text/fbml">
    <% foreach(var i in (List<QuestionYourFriendsDataAccess.Question>)ViewData["questions"])
       {
           %>
           <fb:request-form
                    action="http://apps.facebook.com/questionyourfriends/MyQuestions/Delete"
                    target="_top"
                    method="GET"
                    invite="true"
                    type="Demo"
                    content="Hello"
                    label='Accept' >
            <input type="hidden" value="<%:i.id %>" name="qid"/>
            <fb:submit>X</fb:submit>
            </fb:request-form>
           <fb:request-form
                    action="http://apps.facebook.com/questionyourfriends/MyQuestions/Answeree"
                    target="_top"
                    method="GET"
                    invite="true"
                    type="Demo"
                    content="Hello"
                    label='Accept' >
               <br/>
               <input type="hidden" value="<%:i.id %>" name="qid"/>
                <%:i.text%>
                <br/>
                De <% if (i.anom_price == 0) {%>
                    <%:QuestionYourFriends.Models.Facebook.GetFriendName(i.Owner.fid)%>
                    <% } else { %>
                     <a href="http://apps.facebook.com/questionyourfriends/MyQuestions/Reveal">???</a>
                     <%} %>
                <br/>
                <input type="text" value="" name="answer" />
                <br/>
                  
                <a href="#">annuler</a>
                <fb:submit>répondre</fb:submit>
            </fb:request-form>
            <br/>
    <% } %>
    </script>
    </fb:serverFbml>
</asp:Content>
