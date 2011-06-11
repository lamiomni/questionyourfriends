<%@ Control Language="C#" CodeBehind="ForeignKey.ascx.cs" Inherits="QuestionYourFriendsBackEnd.DynamicData.FieldTemplates.ForeignKeyField" %>

<asp:HyperLink ID="HyperLink1" runat="server"
    Text="<%#GetDisplayString()%>"
    NavigateUrl="<%#GetNavigateUrl()%>"  />

