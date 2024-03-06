$(document).ready(function () {
    //循环模块目录
    var module = requestUrl('/Home/ModuleList', null);
    module = eval("(" + module + ")");
    var content = "";
    for (var i = 0; i < module.total; i++) {
        //获取根节点
        var root = module.rows[i].PARENTID;
        if (module.rows[i].LEVEL == '1') {
            content += "<dd>"
            content += '<div class="title">'
            content += '<span><img src="' + module.rows[i].ICON + '"/></span ><a href="' + module.rows[i].DIRECTORY + '?id=' + module.rows[i].MODULEID + '"  target="rightFrame">' + module.rows[i].MODULE + '</a>';
            content += '</div>';
            content += '<ul class="menuson">'
            var moduleId = module.rows[i].MODULEID;
            var parentId = null;
            for (var j = 0; j < module.total; j++) {
                //遍历进入第二层目录
                if (module.rows[j].LEVEL == '2') {
                    //当第二层的父节点与根节点moduleId相等时，遍历第二层目录
                    parentId = module.rows[j].PARENTID;
                    var moduleId_children = module.rows[j].MODULEID;
                    if (moduleId == parentId) {
                        content += '<li class="' + module.rows[j].STATUS + '">'
                        content += '<div class="header">'
                        content += '<cite></cite>'
                        content += '<a href="' + module.rows[j].DIRECTORY + '?id=' + module.rows[j].MODULEID + '" target="rightFrame">' + module.rows[j].MODULE + '</a>'
                        content += '<i></i>'
                        content += '</div>'
                        //遍历进入第三层目录
                        content += '<ul class="sub-menus">'
                        for (var k = 0; k < module.total; k++) {
                            if (module.rows[k].LEVEL == '3') {
                                parentId = null;
                                parentId = module.rows[k].PARENTID
                                if (moduleId_children == parentId) {
                                    content += '<li><a href="' + module.rows[k].DIRECTORY + '?id=' + module.rows[k].MODULEID + '" target="rightFrame">' + module.rows[k].MODULE + '</a></li>';
                                }
                            }
                        }
                        content += "</ul>";
                        content += '</li>';
                    }
                }
            }
            content += '</ul>';
            content += "</dd>";
        }
    }
    $(".leftmenu").html(content);
    //导航切换
    $(".menuson .header").click(function () {
        var $parent = $(this).parent();
        $(".menuson>li.active").not($parent).removeClass("active open").find('.sub-menus').hide();

        //添加打开目录样式
        $parent.addClass("active");
        if (!!$(this).next('.sub-menus').size()) {
            if ($parent.hasClass("open")) {
                $parent.removeClass("open").find('.sub-menus').hide();
            } else {
                $parent.addClass("open").find('.sub-menus').show();
            }
        }
    });

    // 三级菜单点击
    $('.sub-menus li').click(function (e) {
        $(".sub-menus li.active").removeClass("active")
        $(this).addClass("active");
    });

    $('.title').click(function () {
        var $ul = $(this).next('ul');
        $('dd').find('.menuson').slideUp();
        if ($ul.is(':visible')) {
            $(this).next('.menuson').slideUp();
        } else {
            $(this).next('.menuson').slideDown();
        }
    });
})