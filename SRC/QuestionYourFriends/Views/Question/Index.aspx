<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>
    <%
        var q = (QuestionYourFriendsDataAccess.Question)ViewData["question"];
        
         %>
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
    <div class="question-bloc">
           <fb:request-form
                    action="http://apps.facebook.com/questionyourfriends/MyQuestions/Delete"
                    target="_top"
                    method="GET"
                    invite="true"
                    type="Demo"
                    content="Hello"
                    label='Accept' >
            <input type="hidden" value="<%:q.id %>" name="qid"/>
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
               
                    <% if (q.anom_price > 0) {%>
                        <img src="http://localhost/QuestionYourFriends/Content/annon.jpg" height="52" width="52" alt=""/>
                    <% } else { %>
                        <img src="http://graph.facebook.com/<%:q.Owner.fid %>/picture" height="52" width="52" alt=""/>
                    <%} %> 
                    <div class="question">
                        <input type="hidden" value="<%:q.id %>" name="qid"/>
                        <fb:dialog id="my_dialog-reveal<%:q.id %>" cancel_button=1>
                            <fb:dialog-title>Confirmation</fb:dialog-title>	
                            <fb:dialog-content><form id="my_form">Are you sure?</form></fb:dialog-content>
                            <fb:dialog-button type="button" value="Yes" href="http://apps.facebook.com/questionyourfriends/MyQuestions/Reveal?qid=<%:q.id %>" /> 
                        </fb:dialog>
                        <fb:dialog id="my_dialog-topublic<%:q.id %>" cancel_button=1>
                            <fb:dialog-title>Confirmation</fb:dialog-title>	
                            <fb:dialog-content><form id="my_form">Are you sure?</form></fb:dialog-content>
                            <fb:dialog-button type="button" value="Yes" href="http://apps.facebook.com/questionyourfriends/MyQuestions/Reveal?qid=<%:q.id %>" /> 
                        </fb:dialog>
                        <div class="question-status"><span class="name">
                         <% if (q.anom_price == 0) {%>
                                <%:QuestionYourFriends.Models.Facebook.GetFriendName(q.Owner.fid)%>
                           <% } else { %>
                                <a href="#" clicktoshowdialog="my_dialog-reveal<%:q.id %>">???</a>
                           <%} %> 
                        </span> asked you a 
                        <% if (q.private_price > 0) {%>
                                <span class="privacy">private</span>
                        <% } else { %>
                                <span class="privacy">public</span>
                        <% } %>
                        question.
                        <% if (q.anom_price > 0) {%> <a href="#" clicktoshowdialog="my_dialog-reveal<%:q.id %>">Reveal.</a><% } %>
                        <% if (q.private_price > 0) {%> <a href="#" clicktoshowdialog="my_dialog-topublic<%:q.id %>">Make public.</a> <% } %>
                        </div>
                        <div class="question-sentence"><%:q.text%></div>
                        <div class="answer-bloc">
                            <img src="http://graph.facebook.com/<%:q.Receiver.fid %>/picture" height="33" width="33" alt=""/>
                            <div class="answer">
                                <% if (q.date_answer == null) {%>
                                <div class="answer-status">You did not answer this question yet:</div>
                                <div class="answer-sentence">
                                    <input type="text" value="" name="answer" />
                                    <fb:submit>
                                        Answer
                                    </fb:submit>
                                <% } else { %>
                                    <div class="answer-status">You answered:</div>
                                    <%:q.answer%> <br/> <%:q.date_pub%>
                                <% } %>
                                </div>
                            </div>
                        </div>
                    </div>         
                </fb:request-form>
            </div> 
    </script>
    </fb:serverFbml>
</asp:Content>
