<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TermProject.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container container-page">

        <small class="text-muted">
            <asp:Label ID="lblAdded" runat="server" Text=" "></asp:Label></small>


        <asp:Repeater ID="rptDept" runat="server" OnItemCommand="rptDept_ItemCommand">
            <ItemTemplate>
                <div class="row">
                    <div class="col-12 mt-3">
                        <div class="card">
                            <div class="card-horizontal">
                                <div class="img-square-wrapper">
                                    <a class="lightbox" href='<%# DataBinder.Eval(Container.DataItem,"img_url") %>'><img id="imgProduct" src='<%# DataBinder.Eval(Container.DataItem,"img_url") %>'" width="100" height="100" alt="" /></a>
                                     <asp:Image id="Image1" runat="server" Visible="false" ImageUrl='<%# DataBinder.Eval(Container.DataItem,"img_url") %>' />
                                </div>
                                <div class="card-body">
                                    <h4 class="card-title">
                                        <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label></h4>
                                    <h4 class="card-title">
                                        <asp:Label ID="lblID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Department_id") %>'></asp:Label></h4>
                                    <asp:Label ID="lblMercID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Merchant_id") %>'></asp:Label></h4>
                                    <asp:Button ID="btnViewProducts" Text="View Products" runat="server" CommandName="Products" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
            <ItemTemplate>
                <div class="row">
                    <div class="col-12 mt-3">
                        <div class="card">
                            <div class="card-horizontal">
                                <div class="img-square-wrapper">
                                    <a class="lightbox" href='<%# DataBinder.Eval(Container.DataItem,"imageUrl") %>'><img  src='<%# DataBinder.Eval(Container.DataItem,"imageUrl") %>' width="100" height="100" alt="" /></a>
                                    <asp:Image id="imgProduct" runat="server" Visible="false" ImageUrl='<%# DataBinder.Eval(Container.DataItem,"imageUrl") %>' />
                                </div>
                                <div class="card-body">

                                    <h4 class="card-title">
                                        <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "title") %>'></asp:Label></h4>
                                    <p class="card-text">
                                        <asp:Label ID="lblDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "description") %>'></asp:Label>.</p>
                                    <small class="text-muted">
                                        <asp:Label ID="Label1" runat="server" Text="Price: "></asp:Label></small>
                                    <small class="text-muted">
                                        <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "price") %>'></asp:Label></small>
                                    <small class="text-muted">
                                        <asp:Label ID="lblDeptId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "departmentID") %>'></asp:Label></small>
                                    <small class="text-muted">
                                        <asp:Label ID="lblProductId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "productID") %>'></asp:Label></small>
                                    <asp:Label ID="lblMerchantId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "merchantId") %>'></asp:Label></small>
                                    <br />
                                    <br />
                                    <small class="text-muted">
                                        <asp:Label ID="lblQty" runat="server" Text="Quantity:"></asp:Label></small>
                                    <asp:TextBox ID="txtQuantity" TextMode="Number" runat="server" Text="1"></asp:TextBox>
                                    &nbsp
                            <asp:Button ID="btnAddtoCart" Text="Add To Cart" runat="server" CommandName="Add" />
                                    &nbsp
                            <asp:Button ID="btnAddToWishList" Text="Add To WishList" runat="server" CommandName="AddWish" />

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>





</asp:Content>
