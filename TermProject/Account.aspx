<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="TermProject.Account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row mt-3">
        <div class="col-2"></div>
        <div class="col-8">
            <asp:Label ID="lblWarning" CssClass="alert-danger" runat="server" Visible="False" Font-Size="16" Width="100%"></asp:Label>
        </div>
        <div class="col-2"></div>
    </div>

    <div class="row">
        <div class="col-2">
            <asp:Button ID="btnChangePassword" CssClass="btn btn-success mt-4 ml-5" runat="server" Text="Change Password" Font-Size="16pt" Width="70%" OnClick="btnChangePassword_Click" />
        </div>
        <div class="col-4">
            <asp:Label ID="lblAccountType" runat="server" Text="Account Type" Font-Size="16"></asp:Label>
            <asp:TextBox CssClass="form-control" ID="txtAccountType" runat="server" Width="100%" Font-Size="16pt" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="col-4">
            <asp:Label ID="lblUsername" runat="server" Text="Username" Font-Size="16"></asp:Label>
            <asp:TextBox CssClass="form-control" ID="txtUsername" runat="server" Width="100%" Font-Size="16pt" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="col-2"></div>
    </div>

    <div class="row">
        <div class="col-2">
            <asp:Button ID="btnSubmitNewPassword" CssClass="btn btn-success mt-4 ml-5" runat="server" Text="Submit" Visible="false" Font-Size="16pt" Width="70%" OnClick="btnSubmitNewPassword_Click" />
        </div>
        <div class="col-4">
            <asp:Label ID="lblPassword1" Visible="false" runat="server" Text="New password" Font-Size="16"></asp:Label>
            <asp:TextBox CssClass="form-control" ID="txtPassword1" runat="server" Visible="false" Width="100%" Font-Size="16pt" ReadOnly="false"></asp:TextBox>
        </div>
        <div class="col-4">
            <asp:Label ID="lblPassword2" Visible="false" runat="server" Text="Re-Enter new Password" Font-Size="16"></asp:Label>
            <asp:TextBox CssClass="form-control" ID="txtPassword2" runat="server" Visible="false" Width="100%" Font-Size="16pt" ReadOnly="false"></asp:TextBox>
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
        <div class="col-2">
            <asp:Button ID="btnEdit" CssClass="btn btn-success mt-4 ml-5" runat="server" Text="Edit Info" Font-Size="16pt" Width="70%" OnClick="btnEdit_Click" />
            <asp:Button ID="btnSubmitInfo" CssClass="btn btn-success mt-4 ml-5" runat="server" Text="Submit" Visible="false" Font-Size="16pt" Width="70%" OnClick="btnSubmitInfo_Click" />
        </div>
        <div class="col-3">
            <asp:Label ID="lblName" runat="server" Text="Name" Font-Size="16"></asp:Label>
            <asp:TextBox CssClass="form-control" ID="txtName" runat="server" Width="100%" Font-Size="16pt" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="col-3">
            <asp:Label ID="lblEmail" runat="server" Text="Email" Font-Size="16"></asp:Label>
            <asp:TextBox CssClass="form-control" ID="txtEmail" runat="server" Width="100%" Font-Size="16pt" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="col-2">
            <asp:Label ID="lblPhone" runat="server" Text="Phone#" Font-Size="16"></asp:Label>
            <asp:TextBox CssClass="form-control" ID="txtPhone" runat="server" Width="100%" Font-Size="16pt" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="col-2"></div>
    </div>

    <div class="row">
        <div class="col-2"></div>
        <div class="col-4">
            <h1 id="BillingHeader" runat="server" visible="false">Billing Info</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-2"></div>
        <div class="col-3">
            <asp:Label ID="lblAddress" runat="server" Text="Address" Font-Size="16"></asp:Label>
            <asp:TextBox CssClass="form-control" ID="txtAddress" runat="server" Width="100%" Font-Size="16pt" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="col-3">
            <asp:Label ID="lblCity" runat="server" Text="City" Font-Size="16"></asp:Label>
            <asp:TextBox ReadOnly="True" CssClass="form-control" ID="txtCity" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
        </div>
        <div class="col-1">
            <asp:Label ID="lblState" runat="server" Text="State" Font-Size="16"></asp:Label>
            <asp:TextBox ReadOnly="True" CssClass="form-control" ID="txtState" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
        </div>
        <div class="col-1">
            <asp:Label ID="lblZip" runat="server" Text="Zip Code" Font-Size="16"></asp:Label>
            <asp:TextBox ReadOnly="True" CssClass="form-control" ID="txtZip" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
        </div>
        <div class="col-2"></div>
    </div>

    <div class="row">
        <div class="col-2"></div>
        <div class="col-4">
            <asp:Label ID="lblDesc" Visible="false" runat="server" Text="Description" Font-Size="16"></asp:Label>
            <asp:TextBox CssClass="form-control" Visible="false" ID="txtDesc" runat="server" Width="100%" Font-Size="16pt" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="col-4">
            <asp:Label ID="lblApiURL" Visible="false" runat="server" Text="Api URL" Font-Size="16"></asp:Label>
            <asp:TextBox ReadOnly="True" Visible="false" CssClass="form-control" ID="txtApiURL" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
        </div>
        <div class="col-2"></div>
    </div>

    <div class="row">
        <div class="col-2"></div>
        <div class="col-8">
            <asp:Label ID="lblApiKey" Visible="false" runat="server" Text="ApiKey" Font-Size="16"></asp:Label>
            <asp:TextBox ReadOnly="True" Visible="false" CssClass="form-control" ID="txtAPIKey" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
        </div>
        <div class="col-2"></div>
    </div>

    <div class="row">
        <div class="col-2"></div>
        <div class="col-4">
            <h1 id="ShippingHeader" runat="server" visible="false">Shipping Info</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-2"></div>
        <div class="col-3">
            <asp:Label ID="lblSAddress" Visible="false" runat="server" Text="Address" Font-Size="16"></asp:Label>
            <asp:TextBox ReadOnly="True" Visible="false" CssClass="form-control" ID="txtSAddress" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
        </div>
        <div class="col-3">
            <asp:Label ID="lblSCity" Visible="false" runat="server" Text="City" Font-Size="16"></asp:Label>
            <asp:TextBox ReadOnly="True" Visible="false" CssClass="form-control" ID="txtSCity" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
        </div>
        <div class="col-1">
            <asp:Label ID="lblSState" Visible="false" runat="server" Text="State" Font-Size="16"></asp:Label>
            <asp:TextBox ReadOnly="True" Visible="false" CssClass="form-control" ID="txtSState" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
        </div>
        <div class="col-1">
            <asp:Label ID="lblSZip" Visible="false" runat="server" Text="Zip Code" Font-Size="16pt"></asp:Label>
            <asp:TextBox ReadOnly="True" Visible="false" CssClass="form-control" ID="txtSZip" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
        </div>
        <div class="col-2"></div>
    </div>

    <div class="row mt-3">
        <div class="col-2"></div>
        <div class="col-4">
            <h1>Purchases</h1>
        </div>
        <div class="col-4">
            <h1 id="CreditHeader" runat="server" visible="false">Credit Cards</h1>
        </div>
        <div class="col-2"></div>
    </div>

    <div class="row mt-3">
        <div class="col-2"></div>
        <div class="col-4">
            <div class="p-3" id="Purchases" style="font-size: 18pt; border-radius: 10px; border: 3px solid rgb(214,194,38)" runat="server"></div>
        </div>
        <div class="col-4">
            <div class="p-3" id="CreditCards" style="font-size: 18pt; border-radius: 10px; border: 3px solid rgb(214,194,38)" runat="server">
                <div class="row">
                    <div class="col-12">
                        <asp:Label Visible="false" ID="lblSelectCard" runat="server" Text="Cards on Account" Font-Size="16"></asp:Label>
                        <br />
                        <asp:DropDownList Visible="false" OnSelectedIndexChanged="ddlCard_SelectedIndexChanged" CssClass="dropdown btn btn-info" ID="ddlCredit" runat="server" AutoPostBack="True" Width="100%">
                            <asp:ListItem Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <asp:Label ID="lblCardNum" Visible="false" runat="server" Text="Card Number" Font-Size="16pt"></asp:Label>
                        <asp:TextBox ReadOnly="True" Visible="false" CssClass="form-control" ID="txtCardNum" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
                    </div>
                    <div class="col-6">
                        <asp:Label ID="lblCardType" Visible="false" runat="server" Text="Card Type" Font-Size="16pt"></asp:Label>
                        <asp:TextBox ReadOnly="True" Visible="false" CssClass="form-control" ID="txtCardType" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <asp:Label ID="lblExpMonth" Visible="false" runat="server" Text="Expiration Month" Font-Size="16pt"></asp:Label>
                        <asp:TextBox ReadOnly="True" Visible="false" CssClass="form-control" ID="txtExpMonth" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
                    </div>
                    <div class="col-6">
                        <asp:Label ID="lblExpYear" Visible="false" runat="server" Text="Expiration Year" Font-Size="16pt"></asp:Label>
                        <asp:TextBox ReadOnly="True" Visible="false" CssClass="form-control" ID="txtExpYear" runat="server" Width="100%" Font-Size="16pt"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <asp:CheckBox Visible="false" ID="chxDefault" runat="server" Font-Size="16" Text="Default" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <asp:Button ID="btnSubmitCard" CssClass="btn btn-success mt-4 ml-5" runat="server" Text="Submit" Visible="false" Font-Size="16pt" Width="70%" OnClick="btnSubmitCard_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-2">
            <asp:Button ID="btnEditCard" Visible="false" CssClass="btn btn-success mt-4 ml-5" runat="server" Text="Edit Card" Font-Size="16pt" Width="70%" OnClick="btnEditCard_Click" />
        </div>
    </div>





</asp:Content>
