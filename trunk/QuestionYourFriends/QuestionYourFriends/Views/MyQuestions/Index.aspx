<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" CodeBehind="~/Controllers/MyQuestionsController.cs"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Mes questions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">
    <%:ViewData["test"]%>
    <% for (int i = 0; i < (int)ViewData["questionCount"]; i++)
       {
           %>
           <asp:Button runat="server" ID="Delete" Text="X" />
           <br/>
            <%:((List<string>)ViewData["questions"])[i]%>
            <br/>
            <%:((List<string>)ViewData["friend"])[i]%>
            <br/>
            <asp:TextBox runat="server" ID="textBox" />
            <br/>
            <asp:DropDownList ID="DropDownList" runat="server">
                <asp:ListItem Text="public" Value="public" />
                <asp:ListItem Text="privé" Value="private" />
            </asp:DropDownList>
            <asp:Button runat="server" ID="Cancel" Text="annuler" />
            <asp:Button runat="server" ID="Respond" Text="répondre"/>
            <br/>
    <% } %>
    </form>

</asp:Content>
