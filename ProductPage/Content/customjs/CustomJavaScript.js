// Load all product of parent category
function loadProductOfParentCategory(FBId, shopId, parentId, start, quantity, categoryName, loadMore) {
    if (loadMore == 0) {
        //Set Category Name
        $('#h2CategoryName').empty();
        $('#h2CategoryName').text(categoryName);
        $('#divProductContent').empty();
        $('#divLoadMore').empty();
        $('#loadingSpinner').removeAttr("style");
    }
    $.ajax({
        url: "/GetData/GetAllProductOfChildCategory",
        data: {
            shopId: shopId,
            parentCategoryId: parentId,
            start: start,
            quantity: quantity
        },
        dataType: "json",
        success: function (data) {
            if (data.length != 0) {
                for (var i = 0; i < data.length; i++) {
                    var productItemTemplate = $('#productItemTempate > div.col-sm-4').clone();
                    //Set image for product
                    if (data[i].Urls.length != 0) {
                        productItemTemplate.find("div.product-imitation").css({
                            "height": "195px",
                            "background": "url('" + data[i].Urls[0] + "') center no-repeat",
                            "background-size": "contain"
                        });
                    } else {
                        productItemTemplate.find("div.product-imitation").text("Chưa có hình ảnh").css("height", "195px");
                    }
                    productItemTemplate.find("div.ibox-content.product-box > a").attr("onclick", "viewDetail(" + FBId + "," + shopId + "," + data[i].CategoryId + "," + data[i].Id + ")");
                    // Set tag sale and price
                    // If product has promotion
                    if (data[i].PromotionPrice != null && data[i].PromotionPrice < data[i].Price) {
                        productItemTemplate.find("span.product-price").text("Sale");
                        productItemTemplate.find("div.small.m-t-xs > s.text-muted").text(data[i].Price.formatMoney());
                        productItemTemplate.find("div.small.m-t-xs > h4").text(data[i].PromotionPrice.formatMoney());
                        var priceToSale = data[i].PromotionPrice;
                    }
                    // If product has no promotion
                    if (data[i].PromotionPrice == null) {
                        productItemTemplate.find("span.product-price").hide();
                        productItemTemplate.find("div.small.m-t-xs > h4").text(data[i].Price.formatMoney()).css("color", "black");
                        var priceToSale = data[i].Price;
                    }
                    // Set product name
                    productItemTemplate.find("a.product-name").text(data[i].Name);
                    productItemTemplate.find("a.product-name").attr("onclick", "viewDetail(" + FBId + "," + shopId + "," + data[i].CategoryId + "," + data[i].Id + ")");
                    // Set add to cart function
                    var properties = data[i].Name;
                    if (data[i].IsInStock == true) {
                        if (data[i].TemplateId != null) {
                            var j = i;
                            $.ajax({
                                url: "/GetData/GetTemplateProductByShopAndId",
                                async: false,
                                data: {
                                    id: data[i].TemplateId,
                                    shopId: shopId
                                },
                                dataType: "json",
                                success: function (result) {
                                    if (result.Attr1 != null && result.Attr1.length != 0) {
                                        if (data[j].Attr1 != null) {
                                            properties = properties.concat(" ", data[j].Attr1);
                                        }
                                    };
                                    if (result.Attr2 != null && result.Attr2.length != 0) {
                                        if (data[j].Attr2 != null) {
                                            properties = properties.concat(", ", data[j].Attr2);
                                        }
                                    };
                                    if (result.Attr3 != null && result.Attr3.length != 0) {
                                        if (data[j].Attr3 != null) {
                                            properties = properties.concat(", ", data[j].Attr3);
                                        }
                                    };
                                    if (result.Attr4 != null && result.Attr4.length != 0) {
                                        if (data[j].Attr4 != null) {
                                            properties = properties.concat(", ", data[j].Attr4);
                                        }
                                    };
                                    if (result.Attr5 != null && result.Attr5.length != 0) {
                                        if (data[j].Attr5 != null) {
                                            properties = properties.concat(", ", data[j].Attr5);
                                        }
                                    };
                                    if (result.Attr6 != null && result.Attr6.length != 0) {
                                        if (data[j].Attr6 != null) {
                                            properties = properties.concat(", ", data[j].Attr6);
                                        }
                                    };
                                    if (result.Attr7 != null && result.Attr7.length != 0) {
                                        if (data[j].Attr7 != null) {
                                            properties = properties.concat(", ", data[j].Attr7);
                                        }
                                    };
                                    productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[j].Id + ",\"" + properties + "\",\"" + data[j].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                                },
                                error: function () {
                                    productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[i].Id + ",\"" + properties + "\",\"" + data[i].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                                }
                            });
                        } else {
                            productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[i].Id + ",\"" + properties + "\",\"" + data[i].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                        }
                    } else {
                        productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-danger'>Hết hàng</a>");
                    }
                    // Render product item
                    $('#divProductContent').append(productItemTemplate);

                }
                // Add load more button
                if (data.length == quantity) {
                    $('#divLoadMore').empty();
                    $('#divLoadMore').append("<button class='btn btn-primary' onclick='loadProductOfParentCategory(" + FBId + "," + shopId + "," + parentId + "," + parseInt(start + quantity) + "," + quantity + "," + '"' + categoryName + '"' + ",1)'>Thêm sản phẩm</button>");
                }
                else {
                    if (data.length < quantity) {
                        $('#divLoadMore').empty();
                    }
                };
                $('#loadingSpinner').attr("style", {
                    "display": "none"
                });
            } else {
                $('#loadingSpinner').attr("style", {
                    "display": "none"
                });
                if (loadMore == 0) {
                    $('#divProductContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
                } else {
                    $('#divLoadMore').empty();
                }
            }
        },
        error: function () {
            $('#loadingSpinner').attr("style", {
                "display": "none"
            });
            if (loadMore == 0) {
                $('#divProductContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
            } else {
                $('#divLoadMore').empty();
            }
        }
    })
}

// Load all product of a category
function loadProductByShopAndCategory(FBId, shopId, categoryId, start, quantity, categoryName, loadMore) {
    if (loadMore == 0) {
        //Set Category Name
        $('#h2CategoryName').empty();
        $('#h2CategoryName').text(categoryName);
        $('#divProductContent').empty();
        $('#divLoadMore').empty();
        $('#loadingSpinner').removeAttr("style");
    }
    $.ajax({
        url: "/GetData/GetProductByShopAndCategory",
        data: {
            shopId: shopId,
            categoryId: categoryId,
            start: start,
            quantity: quantity
        },
        dataType: "json",
        success: function (data) {
            if (data.length != 0) {
                for (var i = 0; i < data.length; i++) {
                    var productItemTemplate = $('#productItemTempate > div.col-sm-4').clone();
                    //Set image for product
                    if (data[i].Urls.length != 0) {
                        productItemTemplate.find("div.product-imitation").css({
                            "height": "195px",
                            "background": "url('" + data[i].Urls[0] + "') center no-repeat",
                            "background-size": "contain"
                        });
                    } else {
                        productItemTemplate.find("div.product-imitation").text("Chưa có hình ảnh").css("height", "195px");
                    }
                    productItemTemplate.find("div.ibox-content.product-box > a").attr("onclick", "viewDetail(" + FBId + "," + shopId + "," + data[i].CategoryId + "," + data[i].Id + ")");
                    // Set tag sale and price
                    // Set tag sale and price
                    // If product has promotion
                    if (data[i].PromotionPrice != null && data[i].PromotionPrice < data[i].Price) {
                        productItemTemplate.find("span.product-price").text("Sale");
                        productItemTemplate.find("div.small.m-t-xs > s.text-muted").text(data[i].Price.formatMoney());
                        productItemTemplate.find("div.small.m-t-xs > h4").text(data[i].PromotionPrice.formatMoney());
                        var priceToSale = data[i].PromotionPrice;
                    }
                    // If product has no promotion
                    if (data[i].PromotionPrice == null) {
                        productItemTemplate.find("span.product-price").hide();
                        productItemTemplate.find("div.small.m-t-xs > h4").text(data[i].Price.formatMoney()).css("color", "black");
                        var priceToSale = data[i].Price;
                    }
                    // Set product name
                    productItemTemplate.find("a.product-name").text(data[i].Name);
                    productItemTemplate.find("a.product-name").attr("onclick", "viewDetail(" + FBId + "," + shopId + "," + data[i].CategoryId + "," + data[i].Id + ")");
                    // Set add to cart function
                    var properties = data[i].Name;
                    if (data[i].IsInStock == true) {
                        if (data[i].TemplateId != null) {
                            var j = i;
                            $.ajax({
                                url: "/GetData/GetTemplateProductByShopAndId",
                                async: false,
                                data: {
                                    id: data[i].TemplateId,
                                    shopId: shopId
                                },
                                dataType: "json",
                                success: function (result) {
                                    if (result.Attr1 != null && result.Attr1.length != 0) {
                                        if (data[j].Attr1 != null) {
                                            properties = properties.concat(" ", data[j].Attr1);
                                        }
                                    };
                                    if (result.Attr2 != null && result.Attr2.length != 0) {
                                        if (data[j].Attr2 != null) {
                                            properties = properties.concat(", ", data[j].Attr2);
                                        }
                                    };
                                    if (result.Attr3 != null && result.Attr3.length != 0) {
                                        if (data[j].Attr3 != null) {
                                            properties = properties.concat(", ", data[j].Attr3);
                                        }
                                    };
                                    if (result.Attr4 != null && result.Attr4.length != 0) {
                                        if (data[j].Attr4 != null) {
                                            properties = properties.concat(", ", data[j].Attr4);
                                        }
                                    };
                                    if (result.Attr5 != null && result.Attr5.length != 0) {
                                        if (data[j].Attr5 != null) {
                                            properties = properties.concat(", ", data[j].Attr5);
                                        }
                                    };
                                    if (result.Attr6 != null && result.Attr6.length != 0) {
                                        if (data[j].Attr6 != null) {
                                            properties = properties.concat(", ", data[j].Attr6);
                                        }
                                    };
                                    if (result.Attr7 != null && result.Attr7.length != 0) {
                                        if (data[j].Attr7 != null) {
                                            properties = properties.concat(", ", data[j].Attr7);
                                        }
                                    };
                                    productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[j].Id + ",\"" + properties + "\",\"" + data[j].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                                },
                                error: function () {
                                    productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[i].Id + ",\"" + properties + "\",\"" + data[i].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                                }
                            });
                        } else {
                            productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[i].Id + ",\"" + properties + "\",\"" + data[i].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                        }
                    } else {
                        productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-danger'>Hết hàng</a>");
                    }
                    // Render product item
                    $('#divProductContent').append(productItemTemplate);

                }
                // Add load more button
                if (data.length == quantity) {
                    $('#divLoadMore').empty();
                    $('#divLoadMore').append("<button class='btn btn-primary' onclick='loadProductByShopAndCategory(" + FBId + "," + shopId + "," + categoryId + "," + parseInt(start + quantity) + "," + quantity + "," + '"' + categoryName + '"' + ",1)'>Thêm sản phẩm</button>");
                }
                else {
                    if (data.length < quantity) {
                        $('#divLoadMore').empty();
                    }
                };
                $('#loadingSpinner').attr("style", {
                    "display": "none"
                });
            } else {
                $('#loadingSpinner').attr("style", {
                    "display": "none"
                });
                if (loadMore == 0) {
                    $('#divProductContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
                } else {
                    $('#divLoadMore').empty();
                }
            }
        },
        error: function () {
            $('#loadingSpinner').attr("style", {
                "display": "none"
            });
            if (loadMore == 0) {
                $('#divProductContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
            } else {
                $('#divLoadMore').empty();
            }
        }
    })
}

//Load search result
function loadSearchResult(FBId, shopId, productName, start, quantity, loadMore) {
    if (loadMore == 0) {
        //Set Category Name
        $('#h2CategoryName').empty();
        $('#divProductContent').empty();
        $('#divLoadMore').empty();
        $('#loadingSpinner').removeAttr("style");
    }
    $.ajax({
        url: "/GetData/GetProductByShopAndProductName",
        data: {
            shopId: shopId,
            productName: productName,
            start: start,
            quantity: quantity
        },
        dataType: "json",
        success: function (data) {
            if (data.length != 0) {
                for (var i = 0; i < data.length; i++) {
                    var productItemTemplate = $('#productItemTempate > div.col-sm-4').clone();
                    //Set image for product
                    if (data[i].Urls.length != 0) {
                        productItemTemplate.find("div.product-imitation").css({
                            "height": "195px",
                            "background": "url('" + data[i].Urls[0] + "') center no-repeat",
                            "background-size": "contain"
                        });
                    } else {
                        productItemTemplate.find("div.product-imitation").text("Chưa có hình ảnh").css("height", "195px");
                    }
                    productItemTemplate.find("div.ibox-content.product-box > a").attr("onclick", "viewDetail(" + FBId + "," + shopId + "," + data[i].CategoryId + "," + data[i].Id + ")");
                    // Set tag sale and price
                    // If product has promotion
                    if (data[i].PromotionPrice != null && data[i].PromotionPrice < data[i].Price) {
                        productItemTemplate.find("span.product-price").text("Sale");
                        productItemTemplate.find("div.small.m-t-xs > s.text-muted").text(data[i].Price.formatMoney());
                        productItemTemplate.find("div.small.m-t-xs > h4").text(data[i].PromotionPrice.formatMoney());
                        var priceToSale = data[i].PromotionPrice;
                    }
                    // If product has no promotion
                    if (data[i].PromotionPrice == null) {
                        productItemTemplate.find("span.product-price").hide();
                        productItemTemplate.find("div.small.m-t-xs > h4").text(data[i].Price.formatMoney()).css("color", "black");
                        var priceToSale = data[i].Price;
                    }
                    // Set product name
                    productItemTemplate.find("a.product-name").text(data[i].Name);
                    productItemTemplate.find("a.product-name").attr("onclick", "viewDetail(" + FBId + "," + shopId + "," + data[i].CategoryId + "," + data[i].Id + ")");
                    // Set add to cart function
                    var properties = data[i].Name;
                    if (data[i].IsInStock == true) {
                        if (data[i].TemplateId != null) {
                            var j = i;
                            $.ajax({
                                url: "/GetData/GetTemplateProductByShopAndId",
                                async: false,
                                data: {
                                    id: data[i].TemplateId,
                                    shopId: shopId
                                },
                                dataType: "json",
                                success: function (result) {
                                    if (result.Attr1 != null && result.Attr1.length != 0) {
                                        if (data[j].Attr1 != null) {
                                            properties = properties.concat(" ", data[j].Attr1);
                                        }
                                    };
                                    if (result.Attr2 != null && result.Attr2.length != 0) {
                                        if (data[j].Attr2 != null) {
                                            properties = properties.concat(", ", data[j].Attr2);
                                        }
                                    };
                                    if (result.Attr3 != null && result.Attr3.length != 0) {
                                        if (data[j].Attr3 != null) {
                                            properties = properties.concat(", ", data[j].Attr3);
                                        }
                                    };
                                    if (result.Attr4 != null && result.Attr4.length != 0) {
                                        if (data[j].Attr4 != null) {
                                            properties = properties.concat(", ", data[j].Attr4);
                                        }
                                    };
                                    if (result.Attr5 != null && result.Attr5.length != 0) {
                                        if (data[j].Attr5 != null) {
                                            properties = properties.concat(", ", data[j].Attr5);
                                        }
                                    };
                                    if (result.Attr6 != null && result.Attr6.length != 0) {
                                        if (data[j].Attr6 != null) {
                                            properties = properties.concat(", ", data[j].Attr6);
                                        }
                                    };
                                    if (result.Attr7 != null && result.Attr7.length != 0) {
                                        if (data[j].Attr7 != null) {
                                            properties = properties.concat(", ", data[j].Attr7);
                                        }
                                    };
                                    productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[j].Id + ",\"" + properties + "\",\"" + data[j].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                                },
                                error: function () {
                                    productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[i].Id + ",\"" + properties + "\",\"" + data[i].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                                }
                            });
                        } else {
                            productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[i].Id + ",\"" + properties + "\",\"" + data[i].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                        }
                    } else {
                        productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-danger'>Hết hàng</a>");
                    }
                    // Render product item
                    $('#divProductContent').append(productItemTemplate);

                }
                // Add load more button
                if (data.length >= quantity) {
                    $('#divLoadMore').empty();
                    $('#divLoadMore').append("<button class='btn btn-primary' onclick='loadSearchResult(" + FBId + "," + shopId + ",\"" + productName + "\"," + parseInt(start + quantity) + "," + quantity + ",1)'>Thêm sản phẩm</button>");
                }
                else {
                    if (data.length < quantity) {
                        $('#divLoadMore').empty();
                    }
                };
                $('#loadingSpinner').attr("style", {
                    "display": "none"
                });
            } else {
                $('#loadingSpinner').attr("style", {
                    "display": "none"
                });
                if (loadMore == 0) {
                    $('#divProductContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
                } else {
                    $('#divLoadMore').empty();
                }
            }
        },
        error: function () {
            $('#loadingSpinner').attr("style", {
                "display": "none"
            });
            if (loadMore == 0) {
                $('#divProductContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
            } else {
                $('#divLoadMore').empty();
            }
        }
    })
}

//Load newest product 
function loadNewestProductByShop(FBId, shopId, start, quantity, loadMore) {
    if (loadMore == 0) {
        //Set Category Name
        $('#h2CategoryName').empty();
        $('#h2CategoryName').text("Sản phẩm mới");
        $('#divProductContent').empty();
        $('#divLoadMore').empty();
        $('#loadingSpinner').removeAttr("style");
    }
    $.ajax({
        url: "/GetData/GetNewestProductByShop",
        data: {
            shopId: shopId,
            start: start,
            quantity: quantity
        },
        dataType: "json",
        success: function (data) {
            if (data.length != 0) {
                for (var i = 0; i < data.length; i++) {
                    var productItemTemplate = $('#productItemTempate > div.col-sm-4').clone();
                    //Set image for product
                    if (data[i].Urls.length != 0) {
                        productItemTemplate.find("div.product-imitation").css({
                            "height": "195px",
                            "background": "url('" + data[i].Urls[0] + "') center no-repeat",
                            "background-size": "contain"
                        });
                    } else {
                        productItemTemplate.find("div.product-imitation").text("Chưa có hình ảnh").css("height", "195px");
                    }
                    productItemTemplate.find("div.ibox-content.product-box > a").attr("onclick", "viewDetail(" + FBId + "," + shopId + "," + data[i].CategoryId + "," + data[i].Id + ")");
                    // Set tag sale and price
                    // If product has promotion
                    if (data[i].PromotionPrice != null && data[i].PromotionPrice < data[i].Price) {
                        productItemTemplate.find("span.product-price").text("Sale");
                        productItemTemplate.find("div.small.m-t-xs > s.text-muted").text(data[i].Price.formatMoney());
                        productItemTemplate.find("div.small.m-t-xs > h4").text(data[i].PromotionPrice.formatMoney());
                        var priceToSale = data[i].PromotionPrice;
                    }
                    // If product has no promotion
                    if (data[i].PromotionPrice == null) {
                        productItemTemplate.find("span.product-price").hide();
                        productItemTemplate.find("div.small.m-t-xs > h4").text(data[i].Price.formatMoney()).css("color", "black");
                        var priceToSale = data[i].Price;
                    }
                    // Set product name
                    productItemTemplate.find("a.product-name").text(data[i].Name);
                    productItemTemplate.find("a.product-name").attr("onclick", "viewDetail(" + FBId + "," + shopId + "," + data[i].CategoryId + "," + data[i].Id + ")");
                    // Set add to cart function
                    var properties = data[i].Name;
                    if (data[i].IsInStock == true) {
                        if (data[i].TemplateId != null) {
                            var j = i;
                            $.ajax({
                                url: "/GetData/GetTemplateProductByShopAndId",
                                async: false,
                                data: {
                                    id: data[i].TemplateId,
                                    shopId: shopId
                                },
                                dataType: "json",
                                success: function (result) {
                                    if (result.Attr1 != null && result.Attr1.length != 0) {
                                        if (data[j].Attr1 != null) {
                                            properties = properties.concat(" ", data[j].Attr1);
                                        }
                                    };
                                    if (result.Attr2 != null && result.Attr2.length != 0) {
                                        if (data[j].Attr2 != null) {
                                            properties = properties.concat(", ", data[j].Attr2);
                                        }
                                    };
                                    if (result.Attr3 != null && result.Attr3.length != 0) {
                                        if (data[j].Attr3 != null) {
                                            properties = properties.concat(", ", data[j].Attr3);
                                        }
                                    };
                                    if (result.Attr4 != null && result.Attr4.length != 0) {
                                        if (data[j].Attr4 != null) {
                                            properties = properties.concat(", ", data[j].Attr4);
                                        }
                                    };
                                    if (result.Attr5 != null && result.Attr5.length != 0) {
                                        if (data[j].Attr5 != null) {
                                            properties = properties.concat(", ", data[j].Attr5);
                                        }
                                    };
                                    if (result.Attr6 != null && result.Attr6.length != 0) {
                                        if (data[j].Attr6 != null) {
                                            properties = properties.concat(", ", data[j].Attr6);
                                        }
                                    };
                                    if (result.Attr7 != null && result.Attr7.length != 0) {
                                        if (data[j].Attr7 != null) {
                                            properties = properties.concat(", ", data[j].Attr7);
                                        }
                                    };
                                    productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[j].Id + ",\"" + properties + "\",\"" + data[j].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                                },
                                error: function () {
                                    productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[i].Id + ",\"" + properties + "\",\"" + data[i].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                                }
                            });
                        } else {
                            productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[i].Id + ",\"" + properties + "\",\"" + data[i].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                        }
                    } else {
                        productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-danger'>Hết hàng</a>");
                    }
                    // Render product item
                    $('#divProductContent').append(productItemTemplate);

                }
                // Add load more button
                if (data.length >= quantity) {
                    $('#divLoadMore').empty();
                    $('#divLoadMore').append("<button class='btn btn-primary' onclick='loadSearchResult(" + FBId + "," + shopId + "," + parseInt(start + quantity) + "," + quantity + ",1)'>Thêm sản phẩm</button>");
                }
                else {
                    if (data.length < quantity) {
                        $('#divLoadMore').empty();
                    }
                };
                $('#loadingSpinner').attr("style", {
                    "display": "none"
                });
            } else {
                $('#loadingSpinner').attr("style", {
                    "display": "none"
                });
                if (loadMore == 0) {
                    $('#divProductContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
                } else {
                    $('#divLoadMore').empty();
                }
            }
        },
        error: function () {
            $('#loadingSpinner').attr("style", {
                "display": "none"
            });
            if (loadMore == 0) {
                $('#divProductContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
            } else {
                $('#divLoadMore').empty();
            }
        }
    })
}

//Load sale product 
function loadSaleProductByShop(FBId, shopId, start, quantity, loadMore) {
    if (loadMore == 0) {
        //Set Category Name
        $('#h2CategoryName').empty();
        $('#h2CategoryName').text("Sản phẩm đang khuyến mãi");
        $('#divProductContent').empty();
        $('#divLoadMore').empty();
        $('#loadingSpinner').removeAttr("style");
    }
    $.ajax({
        url: "/GetData/GetSaleProductByShop",
        data: {
            shopId: shopId,
            start: start,
            quantity: quantity
        },
        dataType: "json",
        success: function (data) {
            if (data.length != 0) {
                for (var i = 0; i < data.length; i++) {
                    var productItemTemplate = $('#productItemTempate > div.col-sm-4').clone();
                    //Set image for product
                    if (data[i].Urls.length != 0) {
                        productItemTemplate.find("div.product-imitation").css({
                            "height": "195px",
                            "background": "url('" + data[i].Urls[0] + "') center no-repeat",
                            "background-size": "contain"
                        });
                    } else {
                        productItemTemplate.find("div.product-imitation").text("Chưa có hình ảnh").css("height", "195px");
                    }
                    productItemTemplate.find("div.ibox-content.product-box > a").attr("onclick", "viewDetail(" + FBId + "," + shopId + "," + data[i].CategoryId + "," + data[i].Id + ")");
                    // Set tag sale and price
                    // If product has promotion
                    if (data[i].PromotionPrice != null && data[i].PromotionPrice < data[i].Price) {
                        productItemTemplate.find("span.product-price").text("Sale");
                        productItemTemplate.find("div.small.m-t-xs > s.text-muted").text(data[i].Price.formatMoney());
                        productItemTemplate.find("div.small.m-t-xs > h4").text(data[i].PromotionPrice.formatMoney());
                        var priceToSale = data[i].PromotionPrice;
                    }
                    // If product has no promotion
                    if (data[i].PromotionPrice == null) {
                        productItemTemplate.find("span.product-price").hide();
                        productItemTemplate.find("div.small.m-t-xs > h4").text(data[i].Price.formatMoney()).css("color", "black");
                        var priceToSale = data[i].Price;
                    }
                    // Set product name
                    productItemTemplate.find("a.product-name").text(data[i].Name);
                    productItemTemplate.find("a.product-name").attr("onclick", "viewDetail(" + FBId + "," + shopId + "," + data[i].CategoryId + "," + data[i].Id + ")");
                    // Set add to cart function
                    var properties = data[i].Name;
                    if (data[i].IsInStock == true) {
                        if (data[i].TemplateId != null) {
                            var j = i;
                            $.ajax({
                                url: "/GetData/GetTemplateProductByShopAndId",
                                async: false,
                                data: {
                                    id: data[i].TemplateId,
                                    shopId: shopId
                                },
                                dataType: "json",
                                success: function (result) {
                                    if (result.Attr1 != null && result.Attr1.length != 0) {
                                        if (data[j].Attr1 != null) {
                                            properties = properties.concat(" ", data[j].Attr1);
                                        }
                                    };
                                    if (result.Attr2 != null && result.Attr2.length != 0) {
                                        if (data[j].Attr2 != null) {
                                            properties = properties.concat(", ", data[j].Attr2);
                                        }
                                    };
                                    if (result.Attr3 != null && result.Attr3.length != 0) {
                                        if (data[j].Attr3 != null) {
                                            properties = properties.concat(", ", data[j].Attr3);
                                        }
                                    };
                                    if (result.Attr4 != null && result.Attr4.length != 0) {
                                        if (data[j].Attr4 != null) {
                                            properties = properties.concat(", ", data[j].Attr4);
                                        }
                                    };
                                    if (result.Attr5 != null && result.Attr5.length != 0) {
                                        if (data[j].Attr5 != null) {
                                            properties = properties.concat(", ", data[j].Attr5);
                                        }
                                    };
                                    if (result.Attr6 != null && result.Attr6.length != 0) {
                                        if (data[j].Attr6 != null) {
                                            properties = properties.concat(", ", data[j].Attr6);
                                        }
                                    };
                                    if (result.Attr7 != null && result.Attr7.length != 0) {
                                        if (data[j].Attr7 != null) {
                                            properties = properties.concat(", ", data[j].Attr7);
                                        }
                                    };
                                    productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[j].Id + ",\"" + properties + "\",\"" + data[j].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                                },
                                error: function () {
                                    productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[i].Id + ",\"" + properties + "\",\"" + data[i].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                                }
                            });
                        } else {
                            productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + FBId + "," + shopId + "," + data[i].Id + ",\"" + properties + "\",\"" + data[i].Urls[0] + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a>");
                        }
                    } else {
                        productItemTemplate.find("div.m-t.text-righ").append("<a href='javascript:void(0)' class='btn btn-xs btn-outline btn-danger'>Hết hàng</a>");
                    }
                    // Render product item
                    $('#divProductContent').append(productItemTemplate);

                }
                // Add load more button
                if (data.length >= quantity) {
                    $('#divLoadMore').empty();
                    $('#divLoadMore').append("<button class='btn btn-primary' onclick='loadSaleProductByShop(" + FBId + "," + shopId + "," + parseInt(start + quantity) + "," + quantity + ",1)'>Thêm sản phẩm</button>");
                }
                else {
                    if (data.length < quantity) {
                        $('#divLoadMore').empty();
                    }
                };
                $('#loadingSpinner').attr("style", {
                    "display": "none"
                });
            } else {
                $('#loadingSpinner').attr("style", {
                    "display": "none"
                });
                if (loadMore == 0) {
                    $('#divProductContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
                } else {
                    $('#divLoadMore').empty();
                }
            }
        },
        error: function () {
            $('#loadingSpinner').attr("style", {
                "display": "none"
            });
            if (loadMore == 0) {
                $('#divProductContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
            } else {
                $('#divLoadMore').empty();
            }
        }
    })
}

// View detail product modal
function viewDetail(FBId, shopId, categoryId, productId) {

    $('#productName').empty();
    $('#productPriceDiv').empty();
    $('#tblAttribute').empty();
    $('#productDescription').empty();
    $.ajax({
        url: "/GetData/GetProductByProductId",
        data: {
            shopId: shopId,
            categoryId: categoryId,
            productId: productId
        },
        dataType: "json",
        success: function (data) {
            $('#divProductImage').empty();
            if (data.Urls.length != 0) {
                $('#divProductImage').append("<div class='item active' style='height:546px;background: url(\""+data.Urls[0]+"\") center no-repeat;background-size: contain'></div>");
                for (var i = 1; i < data.Urls.length; i++) {
                    $('#divProductImage').append("<div class='item' style='height:546px;background: url(\"" + data.Urls[i] + "\") center no-repeat;background-size: contain'></div>");
                };
            } else {
                $('#divProductImage').append("<div class='item active'><div style='height:200px; display: block;margin: 0 auto;' class='img-responsive text-center'><h2 style='margin-top:100px'>Chưa có hình ảnh</h2></div></div>");
            }
            // Set Product name
            $('#productName').append(data.Name);
            // Set product price
            if (data.PromotionPrice == null) {
                $('#productPriceDiv').append("<h2 class='product-main-price inline' id='mainPrice'>" + data.Price.formatMoney() + "</h2> &#8363; ");
                var currentPrice = data.Price;
                $('#quantity').val(1);
            } else {
                if (data.Price != null && data.PromotionPrice <= data.Price) {
                    $('#productPriceDiv').append("<s class='text-muted' id='mainPrice'>" + data.Price.formatMoney() + "</s><h2 class='product-main-price inline' style='color:red' id='promotionPrice'>" + data.PromotionPrice.formatMoney() + "</h2> &#8363; ");
                    var currentPrice = data.PromotionPrice;
                    $('#quantity').val(1);
                }
            }
            // Set in stock status
            if (!data.IsInStock) {
                $('#productPriceDiv').append("<h3 class='inline' style='color: red'> (Hết hàng)</h3>");
                $('#quantityAndAddToCart').hide();
            } else {
                $('#quantityAndAddToCart').show();
            }
            // Set description
            if (data.Description != null && data.Description.length > 0) {
                $('#productDescription').append(data.Description);
            } else {
                $('#productDescription').append("Chưa có mô tả cho sản phẩm này.");
            }
            // Add hidden field
            $('#hiddenValue').empty();
            $('#hiddenValue').append("<input id='productDetailId' type='hidden' value=" + data.Id + " />");
            $('#hiddenValue').append("<input id='productDetailUrl' type='hidden' value=" + data.Urls[0] + " />");
            $('#hiddenValue').append("<input id='productDetailPrice' type='hidden' value=" + currentPrice + " />");
            $('#hiddenValue').append("<input id='shopIdViewDetail' type='hidden' value=" + shopId + " />");
            $('#hiddenValue').append("<input id='FBIdViewDetail' type='hidden' value=" + FBId + " />");
            // Create Properties string
            var properties = data.Name;

            // Add data attribute if template != null
            if (data.TemplateId != null) {
                $.ajax({
                    url: "/GetData/GetTemplateProductByShopAndId",
                    data: {
                        id: data.TemplateId,
                        shopId: shopId
                    },
                    dataType: "json",
                    success: function (result) {
                        $('#tblAttribute').append("<col width='30%'><col width='70%'>");
                        if (result.Attr1 != null && result.Attr1.length != 0) {
                            if (data.Attr1 == null) {
                                data.Attr1 == '';
                            } else {
                                properties = properties.concat(" ", data.Attr1, ", ");

                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr1 + "</strong></td><td>" + data.Attr1 + "</td></tr>");
                        };
                        if (result.Attr2 != null && result.Attr2.length != 0) {
                            if (data.Attr2 == null) {
                                data.Attr2 == '';
                            } else {
                                properties = properties.concat(" ", data.Attr2, ", ");
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr2 + "</strong></td><td>" + data.Attr2 + "</td></tr>");
                        };
                        if (result.Attr3 != null && result.Attr3.length != 0) {
                            if (data.Attr3 == null) {
                                data.Attr3 == '';
                            } else {
                                properties = properties.concat(" ", data.Attr3, ", ");
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr3 + "</strong></td><td>" + data.Attr3 + "</td></tr>");
                        };
                        if (result.Attr4 != null && result.Attr4.length != 0) {
                            if (data.Attr4 == null) {
                                data.Attr4 == '';
                            } else {
                                properties = properties.concat(" ", data.Attr4, ", ");
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr4 + "</strong></td><td>" + data.Attr4 + "</td></tr>");
                        };
                        if (result.Attr5 != null && result.Attr5.length != 0) {
                            if (data.Attr5 == null) {
                                data.Attr5 == '';
                            } else {
                                properties = properties.concat(" ", data.Attr5, ", ");
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr5 + "</strong></td><td>" + data.Attr5 + "</td></tr>");
                        };
                        if (result.Attr6 != null && result.Attr6.length != 0) {
                            if (data.Attr6 == null) {
                                data.Attr6 == '';
                            } else {
                                properties = properties.concat(" ", data.Attr6, ", ");
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr6 + "</strong></td><td>" + data.Attr6 + "</td></tr>");
                        };
                        if (result.Attr7 != null && result.Attr7.length != 0) {
                            if (data.Attr7 == null) {
                                data.Attr7 == '';
                            } else {
                                properties = properties.concat(" ", data.Attr7);
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr7 + "</strong></td><td>" + data.Attr7 + "</td></tr>");
                        };
                        $('#hiddenValue').append("<input id='productDetailProperties' type='hidden' value='" + properties + "' />");
                    },
                    error: function () {
                        $('#tblAttribute').append("Chưa có mô tả đặc tính cho sản phẩm này");
                        $('#hiddenValue').append("<input id='productDetailProperties' type='hidden' value='" + properties + "' />");
                    }
                })

            } else {
                $('#tblAttribute').append("Chưa có mô tả đặc tính cho sản phẩm này");
                $('#hiddenValue').append("<input id='productDetailProperties' type='hidden' value='" + properties + "' />");
            }
            $('#viewDetailModal').modal('show');
        },
        error: function () {
            toastr.error("Đã xảy ra lỗi", "Lỗi");
        }
    });
}

// Add to cart
function addToCart(FBId, shopId, productId, properties, url, price, quantity) {
    $.ajax({
        url: "/GetData/AddToCart",
        data: {
            productId: productId,
            properties: properties,
            url: url,
            price: price,
            quantity: quantity,
            FBId: FBId,
            shopId: shopId
        },
        dataType: "text",
        success: function (data) {
            $('#cartQuantity').text(data);
            toastr.success("Sản phẩm đã được thêm vào giỏ hàng", "Thành công");
        },
        error: function () {
            toastr.error("Sản phẩm chưa được thêm vào giỏ hàng", "Thất bại");
        }
    })
}


// Add to cart on view detail
$('#addToCartOnViewDetail').click(function () {
    var productDetailId = $('#productDetailId').val();
    var productDetailProperties = $('#productDetailProperties').val();
    var productDetailUrl = $('#productDetailUrl').val();
    var productDetailPrice = $('#productDetailPrice').val();
    var shopIdViewDetail = $('#shopIdViewDetail').val();
    var FBIdViewDetail = $('#FBIdViewDetail').val();
    var quantity = $('#quantity').val();
    if (productDetailId != null && productDetailProperties != null && productDetailUrl != null && productDetailPrice != null && quantity != null) {
        addToCart(FBIdViewDetail, shopIdViewDetail, productDetailId, productDetailProperties, productDetailUrl, productDetailPrice, quantity);
    }

})

function getCart(FBId, shopId) {
    loadDataToCartModal(FBId, shopId);
    $('#cartModal').modal('show');
}


// Load data to view cart modal
function loadDataToCartModal(FBId, shopId) {
    $('#cartDetailList').empty();
    $('#cartDetailList').append("<div class='spiner-example'><div class='sk-spinner sk-spinner-wave'>" +
    "<div class='sk-rect1'></div>" +
        "<div class='sk-rect2'></div>" +
        "<div class='sk-rect3'></div>" +
        "<div class='sk-rect4'></div>" +
        "<div class='sk-rect5'></div>" +
        "</div>" +
    "</div>");

    $.ajax({
        url: "/GetData/GetCart",
        data: {
            FBId: FBId,
            shopId: shopId
        },
        dataType: "json",
        success: function (cart) {
            if (cart != null && cart.length != 0) {
                $('#cartDetailList').empty();
                var totalPrice = 0;
                for (var i = 0; i < cart.length; i++) {
                    // Clone item template
                    var viewDetailItem = $('#cartDetailItem > div.ibox-content').clone();
                    // Set product name
                    viewDetailItem.find("a.text-navy").text(cart[i].properties);
                    // Set product Url
                    if (cart[i].url != null && cart[i].url.length != 0 && cart[i].url != 'undefined') {
                        viewDetailItem.find("div.cart-product-imitation").css({
                            "height": "96px",
                            "background": "url('" + cart[i].url + "') center no-repeat",
                            "background-size": "contain"
                        })
                    }
                    // Set Price
                    viewDetailItem.find("tbody > tr > td:nth-child(3) > h4").text(cart[i].price.formatMoney());
                    // Add quantity
                    viewDetailItem.find("tbody > tr > td:nth-child(4) > input").val(cart[i].quantity).attr("onchange", "updateQuantity(" + cart[i].price + "," + cart[i].productId + ",this," + FBId + "," + shopId + ")");
                    // Calculate total price
                    viewDetailItem.find("tbody > tr > td:nth-child(5) > h4").text((cart[i].price * cart[i].quantity).formatMoney()).attr("id", "quantityProduct" + cart[i].productId);
                    // Add function remove from cart
                    viewDetailItem.find("tbody > tr > td:nth-child(6) > a.text-muted").attr("onclick", "removeCartItem(" + cart[i].productId + "," + FBId + "," + shopId + ")");
                    // Render cart item
                    $('#cartDetailList').append(viewDetailItem);
                    totalPrice = totalPrice + (cart[i].price * cart[i].quantity);
                }
                if (totalPrice > 0) {
                    var totalPriceInCart = $('#totalPriceInCart > div.ibox-content').clone();
                    totalPriceInCart.attr("id", "totalPriceTable");
                    totalPriceInCart.find("tr > td:nth-child(2) > h4").text(totalPrice.formatMoney());
                    $('#cartDetailList').append(totalPriceInCart);
                }

            } else {
                if (cart.length == 0) {
                    $('#cartDetailList').empty();
                    $('#cartDetailList').append("<div class='ibox-content'><h3 class='text-center'>Chưa có sản phẩm nào trong giỏ hàng</h3></div>")
                }
            }
        },
        error: function () {
            $('#cartDetailList').empty();
            $('#cartDetailList').append("<div class='ibox-content'><h3 class='text-center'>Chưa có sản phẩm nào trong giỏ hàng</h3></div>")
        }
    })
}

// Remove cart item
function removeCartItem(productId, FBId, shopId) {
    $.ajax({
        url: "/GetData/DeleteItemCart",
        data: {
            productId: productId,
            FBId: FBId,
            shopId: shopId
        },
        dataType: "json",
        success: function (cart) {
            if (cart != -1) {
                loadDataToCartModal(FBId, shopId);
                $('#cartQuantity').text(cart);
                toastr.success("Sản phẩm xóa khỏi giỏ hàng giỏ hàng", "Thành công");
            } else {
                toastr.error("Sản phẩm chưa được xóa khỏi giỏ hàng", "Thất bại");
            }
        },
        error: function () {
            toastr.error("Sản phẩm chưa được xóa khỏi giỏ hàng", "Thất bại");
        }
    })
}

// Update product quantity in cart
function updateQuantity(currentPrice, productId, input, FBId, shopId) {
    if (input.value < 1) {
        toastr.warning("Số lượng sản phẩm ít nhất là 1", "Số lượng không hợp lệ");
        input.value = 1;
    }
    $.ajax({
        url: "/GetData/UpdateItemCart",
        data: {
            productId: productId,
            quantity: input.value,
            FBId: FBId,
            shopId: shopId
        },
        success: function (result) {
            $('#quantityProduct' + productId).text((currentPrice * input.value).formatMoney());
            $('#totalPriceTable').find("tr > td:nth-child(2) > h4").text(parseInt(result).formatMoney());
        },
        error: function () {
            input.value = 1;
            toastr.error("Số lượng sản phẩm chưa được cập nhật");
        }
    })
}

//Format money
Number.prototype.formatMoney = function (c, d, t) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 0 : c,
        d = d == undefined ? "," : d,
        t = t == undefined ? "." : t,
        s = n < 0 ? "-" : "",
        i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

//Convert money to number
function formatNumber(money) {
    return number.replace(/\./g, '');
}