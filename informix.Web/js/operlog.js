$(document).ready(function () {
    //默认加载第一页
    initPager(getTotalData("/SystemApplication/GetOperlogCount"), 1);
});
//currentPage当前页码，PageCount当前页显示数量
function init(PageIndex, PageCount) {
    var operlogList = requestUrl("/SystemApplication/GetOperlog", { 'currentPage': PageIndex, 'pageCount': PageCount });
    operlogList = eval(operlogList);
    var content = "";
    for (var i = 0; i < operlogList.length; i++) {
        var writeTime = "";
        //将时间格式化
        writeTime = eval('new ' + (operlogList[i].WriteTime.replace(/\//g, '')));
        writeTime = writeTime.format("yyyy-MM-dd hh:mm:ss");
        content += '<tr><td>' + operlogList[i].Title + '</td><td>' + operlogList[i].Type + '</td><td>' + writeTime + '</td><td>' + operlogList[i].UserName + '</td><td>' + operlogList[i].IPAddress + '</td><td>' + operlogList[i].Url + '</td><td>' + operlogList[i].Contents + '</td><td>' + operlogList[i].Others + '</td></tr>';
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