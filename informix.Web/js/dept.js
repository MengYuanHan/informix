$(document).ready(function () {
    //默认加载第一页
    initPager(getTotalData("/SystemApplication/GetDeptCount"), 1);
});
//currentPage当前页码，PageCount当前页显示数量
function init(PageIndex, PageCount) {
    var deptList = requestUrl("/SystemApplication/GetDept", { 'currentPage': PageIndex, 'pageCount': PageCount });
    deptList = eval(deptList);
    var content = "";
    for (var i = 0; i < deptList.length; i++) {
        content += '<tr>';
        content += '<td>' + deptList[i].DeptId + '</td>';
        content += '<td>' + deptList[i].Dept + '</td>';
        content += '<td>' + deptList[i].ParentId + '</td>';
        content += '<td>' + deptList[i].ShowOrder + '</td>';
        content += '<td>' + deptList[i].DeptLevel + '</td>';
        content += '<td>' + deptList[i].ChildCount + '</td>';
        content += '<td>' + deptList[i].Deletes + '</td>';
        content += '<td><a class="tablelink">添加分组</a>||<a class="tablelink">查看分组</a>||<a class="tablelink" onclick="javascript:deleteDept(' + deptList[i].DeptId + ')">删除</a></td>';
        content += '</tr>';
    }
    $(".tablelist tbody").append(content);
};
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
//弹出添加用户对话框
function showDialog() {
    //显示内容
    var content = ''
    content += '<table border= "1" bordercolor= "#0000FF" width= "300" height= "300" cellpadding= "0" cellspacing= "0">'
    content += '<caption><font size="+1">部门信息</font></caption>'
    content += '<tr>'
    content += '<td>名称:</td>'
    content += '<td >'
    content += '<input id="dept" type="text" class="comuserinfo">'
    content += '</td>'
    content += '</tr>'
    content += '<tr>'
    content += '<tr>'
    content += '<td>级别:</td>'
    content += '<td >'
    content += '<input id="deptlevel" type="text" value="1" class="comuserinfo">'
    content += '</td>'
    content += '</tr>'
    content += '<tr>'
    content += '<td>子节点数:</td>'
    content += '<td >'
    content += '<input id="childcount" type="text" value="0" class="comuserinfo">'
    content += '</td>'
    content += '</tr>'
    content += '<tr>'
    content += '<td>'
    content += '<input class="btn" type="submit" value="确定" onclick="javascript:addDept()">'
    content += '</td>'
    content += '<td>'
    content += '<input class="btn" type="submit" value="取消" onclick=art.dialog.list["deptinfo"].close(); />'
    content += '</td>'
    content += '</tr>'
    content += '</table>'
    content += '';
    //弹出对话框
    art.dialog({
        id: 'deptinfo',
        title: '部门',
        content: content,
        width: 525
    });
    $('select[tip],select[reg],input[reg],input[tip],textarea[tip],textarea[reg]').tooltip();
}
//添加角色
function addDept() {
    var dept = $("#dept").val();
    var deptlevel = $("#deptlevel").val();
    var childcount = $("#childcount").val();

    if (dept == "") {
        $("#dept").addClass("tooltipinputerr");
    }
    else {
        var result_dept = requestUrl("/SystemApplication/AddDept", { 'dept': dept, 'deptlevel': deptlevel, 'childcount': childcount });
        if (result_dept == "true") {
            art.dialog.list['deptinfo'].close();
            art.dialog({ time: 2, content: '部门已添加!', width: 200, height: 125 });
            window.location.href = "/SystemApplication/DeptInfo";
            
        }
        else {
            art.dialog.list['deptinfo'].close();
            art.dialog({ time: 2, content: '部门添加失败!', width: 200, height: 125 });
            window.location.href = "/SystemApplication/DeptInfo";
        }
    }
}
function deleteDept(deptId) {
    if (confirm("确定要删除这条记录吗？")) {
        var result = requestUrl("/SystemApplication/deleteDept", { 'DeptId': deptId });

        if (result == "true") {
            art.dialog({ time: 1.5, content: '删除成功!', width: 200, height: 125 });
            window.location.href = "/SystemApplication/DeptInfo";
        }
        else {
            art.dialog({ time: 1.5, content: '操作异常,删除失败!', width: 200, height: 125 });
            window.location.href = "/SystemApplication/DeptInfo";
        }

    }
}