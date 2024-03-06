$(document).ready(function () {
    //svg图数据
    var treeData = null;

    var module = requestUrl('/Home/ModuleList', null);
    module = eval("(" + module + ")");
    //如果左侧菜单目录不为空
    if (module.total != 0) {
        var json = "[";
        //循环第一层
        for (var i = 0; i < module.total; i++) {
            //根节点
            var root = module.rows[i].PARENTID;
            //如果为第一层
            if (module.rows[i].LEVEL == '1') {
                json += "{";
                json += "'name': '" + module.rows[i].MODULE + "(" + module.rows[i].MODULEID +")',";
                json += "'parent':'信息管理系统(0)','value': 10,'children': [";
                //当前的模块ID
                var moduleId = module.rows[i].MODULEID;
                var parentId = null;
                for (var j = 0; j < module.total; j++) {

                    //如果为第二层
                    if (module.rows[j].LEVEL == '2') {
                        //获取第二层父ID
                        parentId = module.rows[j].PARENTID;
                        var moduleId_children = module.rows[j].MODULEID;
                        //如果第二层父ID等于第一层模块ID
                        if (moduleId == parentId) {
                            json += "{";
                            json += "'name':'" + module.rows[j].MODULE + "(" + module.rows[j].MODULEID+")',";
                            json += "'parent': '" + module.rows[i].MODULE + "','value': 10,'children': [";

                            for (var k = 0; k < module.total; k++) {
                                if (module.rows[k].LEVEL == '3') {
                                    parentId = null;
                                    parentId = module.rows[k].PARENTID;
                                    if (moduleId_children == parentId) {
                                        json += "{";
                                        json += "'name': '" + module.rows[k].MODULE + "(" + module.rows[k].MODULEID +")','value': 5";
                                        json += "},";
                                    }
                                }
                            }
                            json += "]},";
                        }
                    }
                }
                json += "]},";
            }
        }

        json += "]";
        //svg数据[json]
        treeData = [{
            "name": "信息管理系统(0)",
            "parent": "root",
            "value": 10,
            "children": eval("(" + json + ")")
        }];
    }
    else {
        treeData = [{
            "name": "信息管理系统",
            "parent": "root",
            "value": 10,
            "children": [
                {
                    "name": "系统管理",
                    "parent": "信息管理系统",
                    "value": 10,
                    //"children": []
                }
            ]
        }
        ];
    }




    // ************** Generate the tree diagram  *****************
    //定义树图的全局属性（宽高）
    var margin = { top: 10, right: 120, bottom: 20, left: 400 },
        width = 1200 - margin.right - margin.left,
        height = 600 - margin.top - margin.bottom;

    var i = 0,
        duration = 750,//过渡延迟时间
        root;

    var tree = d3.layout.tree()//创建一个树布局
        .size([height, width]);

    var diagonal = d3.svg.diagonal()
        .projection(function (d) { return [d.y, d.x]; });//创建新的斜线生成器

    //声明与定义画布属性
    var svg = d3.select("body").append("svg")
        .attr("width", width + margin.right + margin.left)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    root = treeData[0];//treeData为上边定义的节点属性
    root.x0 = height / 2;
    root.y0 = 0;

    update(root);

    d3.select(self.frameElement).style("height", "750px");

    function update(source) {

        // Compute the new tree layout.计算新树图的布局
        var nodes = tree.nodes(root).reverse(),
            links = tree.links(nodes);

        // Normalize for fixed-depth.设置y坐标点，每层占180px
        nodes.forEach(function (d) { d.y = d.depth * 180; });

        // Update the nodes…每个node对应一个group
        var node = svg.selectAll("g.node")
            .data(nodes, function (d) { return d.id || (d.id = ++i); });//data()：绑定一个数组到选择集上，数组的各项值分别与选择集的各元素绑定

        // Enter any new nodes at the parent's previous position.新增节点数据集，设置位置
        var nodeEnter = node.enter().append("g")  //在 svg 中添加一个g，g是 svg 中的一个属性，是 group 的意思，它表示一组什么东西，如一组 lines ， rects ，circles 其实坐标轴就是由这些东西构成的。
            .attr("class", "node") //attr设置html属性，style设置css属性
            .attr("transform", function (d) { return "translate(" + source.y0 + "," + source.x0 + ")"; })
            .on("click", click);

        //添加连接点---此处设置的是圆圈过渡时候的效果（颜色）
        // nodeEnter.append("circle")
        //   .attr("r", 1e-6);//d 代表数据，也就是与某元素绑定的数据。

        nodeEnter.append("path")
            .style("stroke-width", "2px")
            .style("stroke", "#EB5409")
            .style("fill", "white")
            .attr("d", d3.svg.symbol()
                .size(function (d) {
                    if
                    (d.value <= 9) { return "400"; } else if
                    (d.value >= 9) { return "400"; }
                })
                .type(function (d) {
                    if
                    (d.value <= 9) { return "triangle-up"; } else if
                    (d.value >= 9) { return "circle"; }
                }))
            .attr('class', function (d) {
                if (d.value <= 9) {
                    return 'bling';
                } else {
                    return 'fill_normal';
                }
            });

        //添加标签
        nodeEnter.append("text")
            .attr("x", function (d) { return d.children || d._children ? -13 : 13; })
            .attr("dy", ".35em")
            .attr("text-anchor", function (d) { return d.children || d._children ? "end" : "start"; })
            .text(function (d) { return d.name; })
            .style("fill-opacity", 1e-6);

        // Transition nodes to their new position.将节点过渡到一个新的位置-----主要是针对节点过渡过程中的过渡效果
        //node就是保留的数据集，为原来数据的图形添加过渡动画。首先是整个组的位置
        var nodeUpdate = node.transition()  //开始一个动画过渡
            .duration(duration)  //过渡延迟时间,此处主要设置的是圆圈节点随斜线的过渡延迟
            .attr("r", 10)
            .attr("transform", function (d) { return "translate(" + d.y + "," + d.x + ")"; });
        //更新连接点的填充色
        // nodeUpdate.select("circle")
        // .attr("r", 10)
        // .attr('class',function(d){
        //   if(d.value <= 9){
        //     return 'bling';
        //   }else{
        //     return 'fill_normal';
        //   }
        // });
        nodeUpdate.select("path")
            .style("stroke-width", "2px")
            .style("stroke", "#EB5409")
            .style("fill", "white")
            .attr("d", d3.svg.symbol()
                .size(function (d) {
                    if
                    (d.value <= 9) { return "400"; } else if
                    (d.value >= 9) { return "400"; }
                })
                .type(function (d) {
                    if
                    (d.value <= 9) { return "triangle-up"; } else if
                    (d.value >= 9) { return "circle"; }
                }))
            .attr('class', function (d) {
                if (d.value <= 9) {
                    return 'bling';
                } else {
                    return 'fill_normal';
                }
            });

        nodeUpdate.select("text")
            .style("fill-opacity", 1);

        // Transition exiting nodes to the parent's new position.过渡现有的节点到父母的新位置。
        //最后处理消失的数据，添加消失动画
        var nodeExit = node.exit().transition()
            .duration(duration)
            .attr("transform", function (d) { return "translate(" + source.y + "," + source.x + ")"; })
            .remove();

        nodeExit.select("circle")
            .attr("r", 1e-6);

        nodeExit.select("text")
            .style("fill-opacity", 1e-6);

        // Update the links…线操作相关
        //再处理连线集合
        var link = svg.selectAll("path.link")
            .data(links, function (d) { return d.target.id; });

        // Enter any new links at the parent's previous position.
        //添加新的连线
        link.enter().insert("path", "g")
            .attr("class", "link")
            .attr("d", function (d) {
                var o = { x: source.x0, y: source.y0 };
                return diagonal({ source: o, target: o });  //diagonal - 生成一个二维贝塞尔连接器, 用于节点连接图.
            })
            .style("stroke", function (d) {
                //d包含当前的属性console.log(d)
                return '#ccc';
            });

        // Transition links to their new position.将斜线过渡到新的位置
        //保留的连线添加过渡动画
        link.transition()
            .duration(duration)
            .attr("d", diagonal);

        // Transition exiting nodes to the parent's new position.过渡现有的斜线到父母的新位置。
        //消失的连线添加过渡动画
        link.exit().transition()
            .duration(duration)
            .attr("d", function (d) {
                var o = { x: source.x, y: source.y };
                return diagonal({ source: o, target: o });
            })
            .remove();

        // Stash the old positions for transition.将旧的斜线过渡效果隐藏
        nodes.forEach(function (d) {
            d.x0 = d.x;
            d.y0 = d.y;
        });

    }

    //定义一个将某节点折叠的函数
    //Toggle children on click.切换子节点事件
    function click(d) {
        if (d.children) {
            d._children = d.children;
            d.children = null;
        } else {
            d.children = d._children;
            d._children = null;
        }
        update(d);
    }

    // 定义菜单选项
    var userMenuData = [
        [
            {
                text: "添加",
                func: function () {
                    // id为节点id
                    var id = $(this).children("text").html();
                    id = id.substring(id.indexOf("(") + 1, id.indexOf(")"));
                    showDialog(id);
                }
            },
            {
                text: "修改",
                func: function () {
                    var id = $(this).children("text").html();
                    id = id.substring(id.indexOf("(") + 1, id.indexOf(")"));
                    showEditDialog(id);
                }
            },
            {
                text: "禁用",
                func: function () {
                    var id = $(this).children("text").html();
                    id = id.substring(id.indexOf("(") + 1, id.indexOf(")"));
                   
                }
            },
            {
                text: "删除",
                func: function () {
                    var id = $(this).children("text").html();
                    id = id.substring(id.indexOf("(") + 1, id.indexOf(")"));
                    deleteAppModule(id);
                }
            }
        ]
    ];
    // 事件监听方式添加事件绑定
    $("g").smartMenu(userMenuData, {
        name: "chatRightControl",
        container: "g.node"
    });
});

//弹出添加用户对话框
function showDialog(moduleId) {
    var module = requestUrl("/SystemApplication/GetAppModuleByAppId", { "ModuleId": moduleId })
    module = eval(module);
    //显示内容
    var content = '<div id="appModule">';
    content += '<table border = "1" bordercolor = "#0000FF" width = "400" height = "500" cellpadding = "0" cellspacing = "0" >';
    content += '<caption><font size="+1">应用模块</font></caption>';
    content += '<tr>';
    content += '<td>模块代码:</td>';
    content += '<td>';
    content += '<input id="code" class="comuserinfo" width="200" type="text" name="username" reg="^[\s\S]+$" tip="用户不能为空！">';
    content += '</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>应用名称:</td>';
    content += '<td>';
    content += '<select id="application" disabled="disabled" class="comuserinfo" style=" width:100%;">';
    content += '<option selected="selected" value="' + module[0].APPID + '">' + module[0].APPNAME + '</option>';
    content += '</select>';
    content += '</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>父模块名称:</td>';
    content += '<td>';
    content += '<select id="module" class="comuserinfo" style=" width:100%;" disabled="disabled">';
    content += '<option selected="selected" value="' + module[0].MODULEID + '">' + module[0].MODULE + '</option>';
    content += '</select>';
    content += '</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>子模块名称:</td>';
    content += '<td>';
    content += '<input id="subModule" class="comuserinfo" width="200" type="text" name="module" reg="^[\s\S]+$" tip="密码不能为空！" />';
    content += '</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>url:</td>';
    content += '<td><input id="url" class="comuserinfo" type="text" name="url" /></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>系统模块:</td>';
    content += '<td>是<input type="radio" name="IsSystem" value="1" />否<input type="radio" checked="true" name="IsSystem" value="0" /></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>是否关闭:</td>';
    content += '<td>是<input type="radio" name="close" value="1" />否<input type="radio" checked="true" name="close" value="0" /></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>活动状态:</td>';
    content += '<td>打开<input type="radio" name="status" value="active" />关闭<input type="radio" checked="true" name="status" value="inactive" /></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td></td>';
    content += '<td>';
    content += '<input class="btn" type="submit" value="添加" onclick="javascript:addAppModule()">';
    content += '</td>';
    content += '<td>';
    content += '<input class="btn" type="submit" value="取消" onclick=art.dialog.list["appModule"].close(); />';
    content += '</td>';
    content += '</tr>';
    content += '</table>';
    content += '</div>';
    //弹出对话框
    art.dialog({
        id: 'appModule',
        title: '应用模块',
        content: content,
        width: 525
    });
    $('select[tip],select[reg],input[reg],input[tip],textarea[tip],textarea[reg]').tooltip();
}

//添加模块
function addAppModule() {
    //参数
    var code = $("#code").val();
    var application = $("#application option:selected").val();
    var module = $("#module option:selected").val();
    var subModule = $("#subModule").val();
    var directory = $("#url").val();
    var close = $("input[name='close']:checked").val();
    var IsSystem = $("input[name='IsSystem']:checked").val();
    var status = $("input[name='status']:checked").val();
    //判断是否有空值
    if (code == "" || application == "" || module == "" || subModule == "" || close == "" || status == "") {
        $("#code,#application option:selected,#module option:selected,#subModule,#close,#status").addClass("tooltipinputerr");
    }
    else {
        var result_appmodule = requestUrl("/SystemApplication/AddAppModule", { "code": code, "application": application, "module": module, "subModule": subModule, "directory": directory, "close": close, "IsSystem": IsSystem, "status": status });

        if (result_appmodule == "true") {
            art.dialog({ time: 3, content: '模块已添加!', width: 200, height: 125 });
            art.dialog.list['appModule'].close();
            window.location.href = "/SystemApplication/AppModule";
            //更新左侧目录
            window.parent.frames["leftFrame"].location.reload();
        }
        else {
            art.dialog({ time: 3, content: '添加失败!', width: 200, height: 125 });
            art.dialog.list['appModule'].close();
            window.location.href = "/SystemApplication/AppModule";
        }
    }
}
//显示编辑对话框
function showEditDialog(moduleId) {

    var module = requestUrl("/SystemApplication/GetAppModuleByAppId", { "ModuleId": moduleId })
    module = eval(module);
    //显示内容
    var content = '<div id="EditAppModule">';
    content += '<table border = "1" bordercolor = "#0000FF" width = "400" height = "500" cellpadding = "0" cellspacing = "0" >';
    content += '<caption><font size="+1">应用模块</font></caption>';
    content += '<tr>';
    content += '<td>模块代码:</td>';
    content += '<td>';
    content += '<input id="edit_code" class="comuserinfo" width="200" type="text" name="username" reg="^[\s\S]+$" tip="用户不能为空！" value=' + module[0].CODE + '>';
    content += '</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>应用名称:</td>';
    content += '<td>';
    content += '<select id="edit_application" disabled="disabled" class="comuserinfo" style=" width:100%;">';
    content += '<option selected="selected" value="' + module[0].APPID + '">' + module[0].APPNAME + '</option>';
    content += '</select>';
    content += '</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>父模块名称:</td>';
    content += '<td>';
    content += '<select id="edit_module" disabled="disabled" class="comuserinfo" style=" width:100%;">';
    content += '<option selected="selected" value="' + module[0].P_MODULEID + '">' + module[0].P_MODULE + '</option>';
    content += '</select>';
    content += '</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>子模块名称:</td>';
    content += '<td>';
    content += '<input id="edit_subModule" class="comuserinfo" width="200" type="text" name="module" reg="^[\s\S]+$" tip="密码不能为空！" value="' + module[0].MODULE + '" />';
    content += '</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>url:</td>';
    content += '<td><input id="edit_url" class="comuserinfo" type="text" name="url" value="' + module[0].DIRECTORY + '" /></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>系统模块:</td>';
    content += '<td>是<input type="radio" name="edit_IsSystem" value="1" />否<input type="radio" checked="true" name="edit_IsSystem" value="0" /></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>是否关闭:</td>';
    content += '<td>是<input type="radio" name="edit_close" value="1" />否<input type="radio" checked="true" name="edit_close" value="0" /></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>活动状态:</td>';
    content += '<td>打开<input type="radio" name="edit_status" value="active" />关闭<input type="radio" checked="true" name="edit_status" value="inactive" /></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td></td>';
    content += '<td>';
    content += '<input class="btn" type="submit" value="修改" onclick="javascript:updateAppModule(' + module[0].ModuleId + ')">';
    content += '</td>';
    content += '<td>';
    content += '<input class="btn" type="submit" value="取消" onclick=art.dialog.list["EditAppModule"].close(); />';
    content += '</td>';
    content += '</tr>';
    content += '</table>';
    content += '</div>';
    //弹出对话框
    art.dialog({
        id: 'EditAppModule',
        title: '应用模块',
        content: content,
        width: 525
    });
}
//修改应用模块信息
function updateAppModule(moduleId) {
    //参数
    var code = $("#edit_code").val();
    var application = $("#edit_application option:selected").val();
    var module = $("#edit_module option:selected").val();
    var subModule = $("#edit_subModule").val();
    var directory = $("#edit_url").val();
    var close = $("input[name='edit_close']:checked").val();
    var IsSystem = $("input[name='edit_IsSystem']:checked").val();
    var status = $("input[name='edit_status']:checked").val();

    var result_edit_module = requestUrl("/SystemApplication/UpdateAppModule", { "ModuleId": moduleId, "Code": code, "AppId": application, "ParentId": module, "subModule": subModule, "Directory": directory, "Close": close, "IsSystem": IsSystem, "Status": status });

    if (result_edit_module == "true") {
        art.dialog({ time: 3, content: '已修改!', width: 200, height: 125 });
        art.dialog.list['EditAppModule'].close();
        window.location.href = "/SystemApplication/AppModule";
        //更新左侧目录
        window.parent.frames["leftFrame"].location.reload();
    }
    else {
        art.dialog({ time: 3, content: '修改失败!', width: 200, height: 125 });
        art.dialog.list['appModule'].close();
        window.location.href = "/SystemApplication/AppModule";
    }
}

//更新状态
function updateAppModuleStatus(moduleId, close) {
    var d = requestUrl("/SystemApplication/UpdateAppModuleStatus", { 'moduleId': moduleId, 'close': close });
    if (d == "0") {
        art.dialog({ time: 3, content: '已启用!', width: 200, height: 125 });
        window.location.href = "/SystemApplication/AppModule";
    } else {
        art.dialog({ time: 3, content: '已禁用!', width: 200, height: 125 });
        window.location.href = "/SystemApplication/AppModule";
    }
}
//删除模块
function deleteAppModule(moduleId) {
    if (moduleId == 0) {
        alert("根节点不能删除!");
        return false;
    }
    if (confirm("确定要删除这条记录吗？")) {
        var result_delete_module = requestUrl("/SystemApplication/deleteAppModuleByModuleId", { "ModuleId": moduleId });

        if (result_delete_module == "true") {
            art.dialog({ time: 3, content: '模块已删除!', width: 200, height: 125 });
            window.location.href = "/SystemApplication/AppModule";
            //更新左侧目录
            window.parent.frames["leftFrame"].location.reload();
        }
        else {
            art.dialog({ time: 3, content: '删除失败!', width: 200, height: 125 });
            window.location.href = "/SystemApplication/AppModule";
        }

    }
}