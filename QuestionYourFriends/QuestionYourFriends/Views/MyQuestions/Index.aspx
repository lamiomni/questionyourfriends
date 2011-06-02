<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" CodeBehind="~/Controllers/MyQuestionsController.cs"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Mes questions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <div id="fb-root"></div>
        <script>
            window.fbAsyncInit = function () {
                FB.init({ appId: '174625392585425', status: true, cookie: true,
                    xfbml: true
                });
            };
            (function () {
                var e = document.createElement('script'); e.async = true;
                e.src = document.location.protocol +
              '//connect.facebook.net/en_US/all.js';
                document.getElementById('fb-root').appendChild(e);
            } ());
  
        </script>
    <fb:serverFbml  width="300px" >
    <script type="text/fbml">
    
    <%:ViewData["test"]%>
    <% foreach(var i in (List<QuestionYourFriendsDataAccess.Question>)ViewData["questions"])
       {
           %>
           <fb:request-form
                    action="http://apps.facebook.com/hellototo/MyQuestions/Answeree"
                    target="_top"
                    method="GET"
                    invite="true"
                    type="Demo"
                    content="Hello"
                    label='Accept' >
               <a href="#">Delete</a>
               <br/>
                <%:i.text%>
                <br/>
                <%:QuestionYourFriends.BusinessManagement.Facebook.GetFriendName(i.Owner.fid)%>
                <br/>
                <input type="text" value="" name="answer" />
                <br/>
                <select>
                    <option value="1">public</option>
                    <option value="2">privé</option>                
                </select>
                <input type="hidden" value="<%:i.id %>" name="qid"/> 
                  
                <a href="#"></a>
                <fb:submit >Poser</fb:submit>
            </fb:request-form>
            <br/>
    <% } %>
    </script>
    </fb:serverFbml>
</asp:Content>
