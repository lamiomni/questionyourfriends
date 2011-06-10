<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" CodeBehind="~/Controllers/MyQuestionsController.cs"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    My Questions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width:100%;margin-bottom:40px;">
    </div>
    <ul id="boxes">
    <%
        var received = (ViewData["tab"].ToString() == "toMe") ? "class=actif" : "";
        var sent = (ViewData["tab"].ToString() == "fromMe") ? "class=actif" : "";
    %>
        <li <%:received %>><%:Html.ActionLink("Questions received", "ToMe", "MyQuestions")%></li>
        <li <%:sent %>><%:Html.ActionLink("Questions sent", "fromMe", "Myquestions")%></li>
    </ul>
    <fb:serverFbml  width="670px" >
    
    <script type="text/fbml">
    <style>
    .question-bloc{border-bottom:1px solid #dcdcdc;padding:15px 20px;}
    .question-bloc img{float:left;border:1px solid #C8C8C8;}
    .question-bloc .question{padding-left: 60px;}
    .question-bloc .question .question-status{font-size:12px;color:#646464;}
    .question-bloc .question .question-status .name{font-weight:bold;color:#325587;}
    .question-bloc .question .question-status .privacy{font-weight:bold;color:#558a22;}
    .question-bloc .question .question-sentence{margin-top:4px;font-size:18px;color:#558a22;}
    .question-bloc .question .answer-bloc{background-color:#eeeff4;padding:5px;margin-top: 10px;}
    .question-bloc .question .answer-bloc img{float:left;border:1px solid #C8C8C8;}
    .question-bloc .question .answer-bloc .answer {padding-left: 40px;}
    .question-bloc .question .answer-bloc .answer .answer-status{font-size:11px;color:#646464;}
    .question-bloc .question .answer-bloc .answer .answer-status{font-size:11px;color:#646464;}
    .question-bloc .question .answer-bloc .answer .answer-status .name{font-weight:bold;color:#325587;}
    .question-bloc .question .answer-bloc .answer .answer-status .privacy{font-weight:bold;color:#558a22;}
    .question-bloc .question .answer-bloc .answer .answer-sentence{font-size:11px;color:black;}
    .question-bloc .delete-cross { float: right;}
    </style>
   
    <% foreach(var i in (List<QuestionYourFriendsDataAccess.Question>)ViewData["questions"])
       {
           %>
           <div class="question-bloc">
           <fb:request-form
                    action="http://apps.facebook.com/questionyourfriends/MyQuestions/Delete"
                    target="_top"
                    method="GET"
                    invite="true"
                    type="Demo"
                    content="Hello"
                    label='Accept' >
            <input type="hidden" value="<%:i.id %>" name="qid"/>
            <div class="delete-cross">
                <fb:submit>X</fb:submit>
            </div>
            </fb:request-form>
           <fb:request-form
                    action="http://apps.facebook.com/questionyourfriends/MyQuestions/Answeree"
                    target="_top"
                    method="GET"
                    invite="true"
                    type="Demo"
                    content="Hello"
                    label='Accept' >
               
               
         
                    
                    <% if (i.anom_price > 0) {%>
                        <img src="http://localhost/QuestionYourFriends/Content/annon.jpg" height="52" width="52" alt=""/>
                    <% } else { %>
                        <img src="http://graph.facebook.com/<%:i.Owner.fid %>/picture" height="52" width="52" alt=""/>
                    <%} %> 
                    <div class="question">
                        <input type="hidden" value="<%:i.id %>" name="qid"/>
                        <div class="question-status"><span class="name">
                         <% if (i.anom_price == 0) {%>
                                <%:QuestionYourFriends.Models.Facebook.GetFriendName(i.Owner.fid)%>
                           <% } else { %>
                                 <a href="http://apps.facebook.com/questionyourfriends/MyQuestions/Reveal?qid=<%:i.id %>">???</a>
                           <%} %> 
                        </span> asked you a 
                        <% if (i.private_price > 0) {%>
                                <span class="privacy">private</span>
                        <% } else { %>
                                <span class="privacy">public</span>
                        <% } %>
                        question.
                        <% if (i.anom_price > 0) {%> <a href="http://apps.facebook.com/questionyourfriends/MyQuestions/Reveal?qid=<%:i.id %>">Reveal.</a><% } %>
                        <% if (i.private_price > 0) {%> <a href="http://apps.facebook.com/questionyourfriends/MyQuestions/ToPublic?qid=<%:i.id %>">Make public.</a> <% } %>
                        </div>
                        <div class="question-sentence"><%:i.text%></div>
                        <div class="answer-bloc">
                            <img src="http://graph.facebook.com/<%:i.Receiver.fid %>/picture" height="33" width="33" alt=""/>
                            <div class="answer">
                                <% if (i.date_answer == null) {%>
                                <div class="answer-status">You did not answer this question yet:</div>
                                <div class="answer-sentence">
                                    <input type="text" value="" name="answer" />
                                    <fb:submit>
                                        Answer
                                    </fb:submit>
                                <% } else { %>
                                    <div class="answer-status">You answered:</div>
                                    <%:i.answer%> <br/> <%:i.date_pub%>
                                <% } %>
                                </div>
                            </div>
                        </div>
                    </div>         
                </fb:request-form>
            </div> 
    <% } %>
    </script>
    </fb:serverFbml>
</asp:Content>
