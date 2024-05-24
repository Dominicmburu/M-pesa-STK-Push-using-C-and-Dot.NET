<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="mpesa.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style>
            /* Custom CSS for centering content */
            .center-horizontal {
                display: flex;
                justify-content: center;
                align-items: center;
                min-height: 100vh;
            }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-12">
        <div class="row justify-content-center center-horizontal">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title text-center">M-Pesa Payment</h5>
                        <div class="form-group">
                            <label for="txtPhoneNumber">Phone Number:</label>
                            <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <label for="txtAmount">Amount:</label>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" />
                        </div>
                        <div class="text-center">
                            <asp:Button ID="btnPay" runat="server" Text="Pay using M-Pesa" CssClass="btn btn-primary" OnClick="btnPay_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
