﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="WishList.aspx.cs" Inherits="TermProject.WishList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container container-page">
        <asp:Label ID="lblCartEmpty" runat="server" Text=" "></asp:Label>
        
        
        <br />
        <asp:Button ID ="btnEmpty" runat="server" OnClick="btnEmpty_Click" Text="Empty WishList" />
        <br />
     <asp:Repeater ID="rptProducts" OnItemCommand="rptProducts_ItemCommand" runat="server"  >
        <ItemTemplate>
        <div class="row">
            <div class="col-12 mt-3">
                <div class="card">
                    <div class="card-horizontal">
                        <div class="img-square-wrapper">
                             
                            <asp:Image ID="imgProduct" Height="100" Width="100" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem,"imageUrl") %>' />
                        </div>
                        <div class="card-body">
                            <h4 class="card-title"><asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "title") %>'></asp:Label></h4>
                             <p class="card-text"><asp:Label ID="lblDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "description") %>'></asp:Label>.</p>
                            <small class="text-muted"><asp:Label ID="Label1" runat="server" Text="Price: "></asp:Label></small>
                            <small class="text-muted"><asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "price") %>'></asp:Label></small>
                            <small class="text-muted"><asp:Label ID="lblDeptId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "departmentID") %>'></asp:Label></small>
                            <small class="text-muted"><asp:Label ID="lblProductId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "productID") %>'></asp:Label></small>
                            <br />
                            <br />
                            <small class="text-muted"><asp:Label ID="Label2" runat="server" Text="Quantity: "></asp:Label></small>
                            <small class="text-muted"><asp:TextBox ID="txtQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "quantity") %>'></asp:TextBox></small>
                             <small class="text-muted"><asp:Label ID="lblMerchantId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "merchantId") %>'></asp:Label></small>
                           <br />
                            <br />
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CommandName="Update" />
                            &nbsp
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" />
                            &nbsp
                            <asp:Button ID="btnAdd" runat="server" Text="Add To Cart" CommandName="Add" />

                            
                            
                           
                        </div>
                    </div>
                    
                </div>
            </div>
        </div>
        </ItemTemplate>
         </asp:Repeater>
        </div>
</asp:Content>
