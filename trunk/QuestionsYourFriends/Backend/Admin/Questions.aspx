<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Questions.aspx.cs" Inherits="Backend.Admin.Questions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
    DataKeyNames="id" DataSourceID="EntityDataSource1" ForeColor="#333333" 
    GridLines="None">
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    <Columns>
        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
        <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True" 
            SortExpression="id" />
        <asp:BoundField DataField="id_owner" HeaderText="id_owner" 
            SortExpression="id_owner" />
        <asp:BoundField DataField="id_receiver" HeaderText="id_receiver" 
            SortExpression="id_receiver" />
        <asp:BoundField DataField="text" HeaderText="text" SortExpression="text" />
        <asp:BoundField DataField="answer" HeaderText="answer" 
            SortExpression="answer" />
        <asp:BoundField DataField="anom_price" HeaderText="anom_price" 
            SortExpression="anom_price" />
        <asp:BoundField DataField="private_price" HeaderText="private_price" 
            SortExpression="private_price" />
        <asp:CheckBoxField DataField="undesirable" HeaderText="undesirable" 
            SortExpression="undesirable" />
        <asp:BoundField DataField="date_pub" HeaderText="date_pub" 
            SortExpression="date_pub" />
        <asp:BoundField DataField="date_answer" HeaderText="date_answer" 
            SortExpression="date_answer" />
    </Columns>
    <EditRowStyle BackColor="#999999" />
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
<asp:EntityDataSource ID="EntityDataSource1" runat="server" 
    ConnectionString="name=QuestionYourFriendsEntities" 
    DefaultContainerName="QuestionYourFriendsEntities" EnableDelete="True" 
    EnableFlattening="False" EnableInsert="True" EnableUpdate="True" 
    EntitySetName="Questions">
</asp:EntityDataSource>
</asp:Content>
