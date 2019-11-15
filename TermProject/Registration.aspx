<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="TermProject.Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="row">
            <div class="col-2"></div>
            <div class="col-4">
                <h1>Register Page</h1>
            </div>
        </div>
    <div class="col-2"></div>
    <div id="container col-8" class="m-3" style="border-radius: 10px; border: 3px solid rgb(214,194,38)">
        
        <br />
        <div class="row">

            <div class="col-4"></div>
            <div class="col-4">
                <asp:Label ID="lblUserType" runat="server" Text="Select User Type" Font-Size="16"></asp:Label>
                <br />
                <asp:DropDownList OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged" CssClass="dropdown btn btn-info" ID="ddlUserType" runat="server" AutoPostBack="True" Width="100%">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="1">Customer</asp:ListItem>
                    <asp:ListItem Value="2">Merchant</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-4"></div>
        </div>
        <div class="row mt-3 mb-3">
            <div class="col-2"></div>
            <div class="col-8">
                <asp:Label ID="lblWarning" CssClass="alert-danger" runat="server" Visible="False" Font-Size="16"></asp:Label>
            </div>
            <div class="col-2"></div>
        </div>
        <div class="row">
            <div class="col-2"></div>
            <div class="col-4">
                <h1>User Info</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-2"></div>
            <div class="col-4">
                <asp:Label ID="lblUsername" runat="server" Text="Username" Font-Size="16"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtUsername" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
            </div>
            <div class="col-2">
                <asp:Label ID="lblPassword1" runat="server" Text="Password" Font-Size="16"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtPassword1" runat="server" Width="100%" Font-Size="16pt" TextMode="Password"></asp:TextBox>
            </div>
            <div class="col-2">
                <asp:Label ID="lblPassword2" runat="server" Text="Re-enter Password" Font-Size="16"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtPassword2" runat="server" Width="100%" Font-Size="16pt" TextMode="Password"></asp:TextBox>
            </div>
            <div class="col-2"></div>
        </div>

        <div class="row">
            <div class="col-2"></div>
            <div class="col-4">
                <h1>Personal Info</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-3"></div>
            <div class="col-2">
                <asp:Label ID="lblName" runat="server" Text="Name" Font-Size="16"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtName" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
            </div>
            <div class="col-2">
                <asp:Label ID="lblEmail" runat="server" Text="Email" Font-Size="16"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtEmail" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
            </div>
            <div class="col-2">
                <asp:Label ID="lblPhone" runat="server" Text="Phone#" Font-Size="16"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtPhone" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
            </div>
            <div class="col-3"></div>
        </div>
        <div class="row">
            <div class="col-2"></div>
            <div class="col-4">
                <h1>Address Info</h1>
            </div>
        </div>

        <div class="row">
            <div class="col-2"></div>
            <div class="col-4">
                <asp:Label ID="lblBillingInfo" runat="server" Text="Billing Address" Font-Size="24pt" Visible="False"></asp:Label>
            </div>
        </div>

        <div class="row">
            <div class="col-2"></div>
            <div class="col-3">
                <asp:Label ID="lblAddress" runat="server" Text="Address" Font-Size="16"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtAddress" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
            </div>
            <div class="col-3">
                <asp:Label ID="lblCity" runat="server" Text="City" Font-Size="16"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtCity" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
            </div>
            <div class="col-1">
                <asp:Label ID="lblState" runat="server" Text="State" Font-Size="16"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtState" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
            </div>
            <div class="col-1">
                <asp:Label ID="lblZip" runat="server" Text="Zip Code" Font-Size="16"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtZip" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
            </div>
            <div class="col-2"></div>
        </div>

        <div class="row">
            <div class="col-2"></div>
            <div class="col-4">
                <asp:Label ID="lblShippingInfo" runat="server" Text="Shipping Address" Font-Size="24pt" Visible="False"></asp:Label>
            </div>
        </div>

        <div class="row">
            <div class="col-2"></div>
            <div class="col-3">
                <asp:Label ID="lblSAddress" runat="server" Text="Address" Font-Size="16" Visible="False"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtSAddress" runat="server" Width="100%" Font-Size="16pt" Visible="False"></asp:TextBox>
            </div>
            <div class="col-3">
                <asp:Label ID="lblSCity" runat="server" Text="City" Font-Size="16" Visible="False"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtSCity" runat="server" Width="100%" Font-Size="16pt" Visible="False"></asp:TextBox>
            </div>
            <div class="col-1">
                <asp:Label ID="lblSState" runat="server" Text="State" Font-Size="16" Visible="False"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtSState" runat="server" Width="100%" Font-Size="16pt" Visible="False"></asp:TextBox>
            </div>
            <div class="col-1">
                <asp:Label ID="lblSZip" runat="server" Text="Zip Code" Font-Size="16pt" Visible="False"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtSZip" runat="server" Width="100%" Font-Size="16pt" Visible="False"></asp:TextBox>
            </div>
            <div class="col-2"></div>
        </div>

        <div class="row">
            <div class="col-2"></div>
            <div class="col-4">
                <asp:Label ID="lblMerchantInfo" runat="server" Text="Merchant Information" Font-Size="24pt" Visible="False"></asp:Label>
            </div>
        </div>

        <div class="row">
            <div class="col-2"></div>
            <div class="col-3">
                <asp:Label ID="lblDescription" runat="server" Text="Merchant Description" Font-Size="16" Visible="False"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtDesc" runat="server" Width="100%" Font-Size="16pt" Visible="False"></asp:TextBox>
            </div>
            <div class="col-3">
                <asp:Label ID="lblApiKey" runat="server" Text="Api-Key" Font-Size="16" Visible="False"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtApiKey" runat="server" Width="100%" Font-Size="16pt" Visible="False" ReadOnly="True"></asp:TextBox>
            </div>
            <div class="col-2">
                <asp:Label ID="lblGenerateKey" runat="server" Text="Generate Api-Key" Font-Size="16" Visible="False"></asp:Label>
                <br />
                <asp:Button ID="btnGenerateKey" CssClass="btn btn-warning" Font-Size="16" runat="server" Text="Generate" Visible="False" OnClick="btnGenerateKey_Click" />
            </div>
            <div class="col-2"></div>

        </div>

        <div class="row mt-5">
            <div class="col-2"></div>
            <div class="col-8">
                <asp:Label ID="lblAPIURL" runat="server" Text="Api URL" Font-Size="16" Visible="False"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtAPIURL" runat="server" Width="100%" Font-Size="16pt" Visible="False"></asp:TextBox>
            </div>
            <div class="col-2"></div>
        </div>

        <div class="row mt-5 mb-5">
            <div class="col-2"></div>
            <div class="col-8">
                <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" Text="Register" Font-Size="16pt" Visible="False" Width="100%" OnClick="btnSubmit_Click" />
            </div>
            <div class="col-2"></div>
        </div>
    </div>
    <div class="col-2"></div>

</asp:Content>
