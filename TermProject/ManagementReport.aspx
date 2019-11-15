<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ManagementReport.aspx.cs" Inherits="TermProject.ManagementReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <h3>Customer Sales</h3>
            <br />
            <asp:GridView ID="gvCustomerSales" style="font-size: 1.5em; border-radius:10px; border: 3px solid rgb(214,194,38)" runat="server" AutoGenerateColumns="False" >
                <HeaderStyle BackColor="Silver"
                    ForeColor="Black" />
                <RowStyle BackColor="#F0F0F0"
                    ForeColor="black"
                    Font-Italic="true" />

                <AlternatingRowStyle BackColor="Silver"
                    ForeColor="black"
                    Font-Italic="true" />
                <Columns>

                   <asp:BoundField DataField="username" HeaderText="Username" ReadOnly="true" />

                    <asp:BoundField DataField="name" HeaderText="Customer Name" ReadOnly="true" />

                    <asp:BoundField DataField="email" HeaderText="Email Address" ReadOnly="true" />

                    <asp:BoundField DataField="totalsales" HeaderText="Total Dollar Sales" ReadOnly="true" />

                    
                </Columns>
            </asp:GridView>

        <br />
        <h3>Invetory Report</h3>
        <br />
           
            <asp:GridView ID="gvInvetory" style="font-size: 1.5em; border-radius:10px; border: 3px solid rgb(214,194,38)" runat="server" AutoGenerateColumns="False" >
                <HeaderStyle BackColor="Silver"
                    ForeColor="Black" />
                <RowStyle BackColor="#F0F0F0"
                    ForeColor="black"
                    Font-Italic="true" />

                <AlternatingRowStyle BackColor="Silver"
                    ForeColor="black"
                    Font-Italic="true" />
                <Columns>

                   <asp:BoundField DataField="departmentID" HeaderText="Department" ReadOnly="true" />

                    <asp:BoundField DataField="description" HeaderText="Description" ReadOnly="true" />

                    <asp:BoundField DataField="price" HeaderText="Price" ReadOnly="true" />

                    <asp:BoundField DataField="quantity" HeaderText="Quantity" ReadOnly="true" />

                    
                </Columns>
            </asp:GridView> 
         <br />
        <h3>Sales Report</h3>
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
               
                </div>
                
            </div>
        </div>
        
    
        <br />
        </div>






</asp:Content>
