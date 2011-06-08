<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" CodeBehind="~/Controllers/MyQuestionsController.cs"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Mes questions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="question-bloc">
        <div class="questionner-pic">
            <img src="" height="52" width="52" alt=""/>
        </div>
        <div class="question">
            <div class="question-status"><span class="name">Antony</span> a posé une question a <span class="name">Antony</span> en <span class="privacy">public</span></div>
            <div class="question-sentence">Qui es-tu ?</div>
            <div class="answer-bloc">
                <div class="answer-pic">
                    <img src="" height="33" width="33" alt="" />
                </div>
                <div class="answer">
                    <div class="answer-status"><span class="name">Antony</span> vous a répondu en <span class="privacy">public</span> - <a href="">Rendre public</a></div>
                    <div class="answer-sentence">Je suis Antony</div>
                </div>
            </div>
        </div>
    </div>




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
               <% if (i.anom_price == 0) {%>
                    <%:QuestionYourFriends.Models.Facebook.GetFriendName(i.Owner.fid)%>
               <% } else { %>
                     <a href="http://apps.facebook.com/questionyourfriends/MyQuestions/Reveal?qid=<%:i.id %>">???</a>
               <%} %> vous a posé une question en 
               <% if (i.private_price > 0) {%>
                     privé . <a href="http://apps.facebook.com/questionyourfriends/MyQuestions/ToPublic?qid=<%:i.id %>">Rendre public</a>
               <% } else { %>
                     public
               <% } %>
                <br/>
                <%:i.text%>
                <br/>
                <% if (i.date_answer == null) {%>
                    <input type="text" value="" name="answer" />
                <% } else { %>
                    Vous avez répondu
                </br>
                <%:i.text%> <br/> <%:i.date_pub%>
                <br/>
                <% } %>
                <fb:submit>
                <% if (i.date_answer == null) {%>
                    répondre
                <% } %>
                </fb:submit>
                    
            </fb:request-form>
            <br/>
    <% } %>
    </script>
    </fb:serverFbml>
</asp:Content>
