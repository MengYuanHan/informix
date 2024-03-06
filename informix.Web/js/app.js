
$(document).ready(function () {
    initPager(getTotalData("/SystemApplication/GetAppCount"), 1);
});

function init(PageIndex, PageCount) {
    var application = requestUrl("/SystemApplication/GetApp", { 'currentPage': PageIndex, 'pageCount': PageCount });
    application = eval(application);
    var content = "";
    for (var i = 0; i < application.length; i++) {
        content += '<tr><td><input name="" type="checkbox" value="" /></td>';
        content += '<td>' + application[i].AppId + '</td>';
        content += '<td>' + application[i].AppName + '</td>';
        content += '<td>' + application[i].AppDesc + '</td>';
        content += '<td>' + application[i].Url + '</td>';
        content += '<td>' + application[i].OrderBy + '</td>';
        content += '<td><a class="tablelink" onclick="javascript:getAppById(' + application[i].AppId + ')">查看</a>   <a class="tablelink" onclick="javascript:deleteApp(' + application[i].AppId + ')">删除</a></td></tr>';
    }
    $('.tablelist tbody').html(content);
}
//总记录数 、当前页码
function initPager(RecordCount, PageIndex) {
    $("#page-bottom").setPager({
        RecordCount: RecordCount,
        PageIndex: PageIndex,
        buttonClick: function (RecordCount, PageIndex) {
            $(".tablelist tbody").html("");
            initPager(RecordCount, PageIndex);
        }
    });
    //分页同时更新数据源
    init(PageIndex, 15);
};
function showDialog() {
    //显示内容
    var content = $("#appList").html();
    //弹出对话框
    art.dialog({
        id: 'appList',
        title: '应用列表',
        content: content,
        width: 525
    });
    //$('select[tip],select[reg],input[reg],input[tip],textarea[tip],textarea[reg]').tooltip();
}
//添加应用
function addApp() {
    var appName = $("#appName").val();
    var decription = $("#decription").val();
    var url = $("#appurl").val();
    var orders = $("#apporders").val();
    var icon = $("#icon option:selected").val();
    if (appName == "") {
        alert("应用名称不能为空");
        return false;
    }
    if (orders == "") {
        alert("排序不能为空");
        return false;
    }
    var result = requestUrl("/SystemApplication/AddApp", { 'appName': appName, 'decription': decription, 'appurl': url, 'apporders': orders, 'icon': icon });
    if (result == "1") {
        art.dialog.list['appList'].close();
        art.dialog({ time: 1.5, content: '添加成功!', width: 200, height: 125 });
        initPager(getTotalData("/SystemApplication/GetAppCount"), 1);
        //更新左侧目录
        window.parent.frames["leftFrame"].location.reload();
    }
    else {
        art.dialog({ time: 1.5, content: '操作异常,添加失败!', width: 200, height: 125 });
    }
}

//删除应用
function deleteApp(applicationId) {
    if (confirm("确定要删除这条记录吗？")) {
        var result = requestUrl("/SystemApplication/DeleteApp", { 'ApplicationId': applicationId });

        if (result == "1") {
            art.dialog({ time: 1.5, content: '删除成功!', width: 200, height: 125 });
            initPager(getTotalData("/SystemApplication/GetAppCount"), 1);
            //更新左侧目录
            window.parent.frames["leftFrame"].location.reload();
        }
        else if (result == "0") {
            art.dialog({ time: 1.5, content: '您没有删除权限!', width: 200, height: 125 });
        }
        else {
            art.dialog({ time: 1.5, content: '操作异常,删除失败!', width: 200, height: 125 });
        }

    }
}
//通过应用编号查询对应模块
function getAppById(applicationId) {
    var result = requestUrl("/SystemApplication/GetAppById", { 'ApplicationId': applicationId });
    result = eval("(" + result + ")");
    var content = '<div><table style="width:100%;"><tr><td style="width:75px; height:30px; text-align:center; border:1px solid gray;">应用名称</td><td style="width:75px; height:30px; text-align:center; border:1px solid gray;">模块名称</td><td style="width:75px; height:30px; text-align:center; border:1px solid gray;">路径</td></tr>';
    for (var j = 0; j < result.total; j++) {
        content += '<tr><td style="width:75px; height:30px; text-align:center; border:1px solid gray;">' + result.rows[j].APPNAME + '</td><td style="width:75px; height:30px; text-align:center; border:1px solid gray;">' + result.rows[j].MODULE + '</td><td style="width:75px; height:30px; text-align:center; border:1px solid gray;">' + result.rows[j].DIRECTORY + '</td>';
    }
    content += '</table></div>';
    art.dialog({
        width: 450,
        height: 200,
        title: '应用模块',
        content: content
    });
}