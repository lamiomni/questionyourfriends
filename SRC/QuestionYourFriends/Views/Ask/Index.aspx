<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Page d'accueil
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="margin:auto;margin-top:30px;text-align:center;">
    <fb:serverFbml  width="500px"  >
    <script type="text/fbml">
        <style>
            input[idname=friend_sel] {width:302px;margin-left:2px; }
            input[name=ask] { }
            input[name=private_cost] {margin-left:2px;width:308px;border: 1px solid #BDC7D8;}
            input[name=annon_cost] {margin-left:2px;width:308px;border: 1px solid #BDC7D8;}
            .uiButtonConfirm { width:60px;float:right;margin-right:3px;}
            .uiButtonConfirm a {color: white;}
        </style>
        <fb:fbml>
            <div class="pas UIContentBox uiBoxLightblue">
            <fb:request-form
            action="http://apps.facebook.com/questionyourfriends/Ask/Ask"
                    target="_top"
                    method="GET"
                    invite="true"
                    type="Demo"
                    content="Hello"
                    label='Accept' >
                <table>
                    <tr>
                        <td style="text-align:right;"><label for="friend">Friend's name:</label></td>
                        <td><fb:friend-selector name="uid" idname="friend_sel" id="friend"/></td>
                    </tr>
                    <tr>
                        <td style="text-align:right;"><label for="question">Formulate your question:</label></td>
                        <td><textarea name="ask" rows="5" cols="57" id="question" ></textarea></td>
                    </tr>
                    <tr>
                        <td style="text-align:right;"><label for="annon_cost">Anonymize price:</label></td>
                        <td><input type="text" name="annon_cost" id="annon_cost" value="0"/></td>
                    </tr>
                    <tr>
                        <td style="text-align:right;"><label for="private_cost">Privatize price:</label></td>
                        <td><input type="text" name="private_cost" id="private_cost" value="0"/></td>
                    </td>
                    <tr>
                        <td></td>
                        <td><label class="fbCommentButton uiButton uiButtonConfirm"><fb:submit>Send</fb:submit></label></td>
                    </tr>
                </table>
                
            </fb:request-form>
            </div>
      </fb:fbml>
    </script>
  </fb:serverFbml>
  </div>
</asp:Content>
