<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="TermProject.LogIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row mt-5">
        <div class="col-4"></div>
        <div class="col-4">
            <div class="container p-3" style="font-size: 1.5em; border-radius:10px; border: 3px solid rgb(214,194,38)">
                <asp:Login ID="Login1" runat="server" OnAuthenticate="UserLogin"></asp:Login>
            </div>
        </div>
        <div class="col-4"></div>
    </div>

</asp:Content>
