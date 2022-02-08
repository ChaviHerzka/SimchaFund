$(() => {
    $("#new-contributor").on('click', function () {
        $("#addModal").modal()
    })

    $("#search").on('keyup', function () {
        let search = $(this).val();
        $("tbody tr").each(function () {
            let row = $(this);
            let cell = row.find("#name")
            if (!cell.text().toLowerCase().includes(search.toLowerCase())) {
                row.hide();
            }
            else {
                row.show();
            }
        })
    })
    $("#clear").on('click', function () {
        $("#search").val('');
        $(".rows").show();
    })
})
