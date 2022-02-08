$(() => {
    $(".edit-contributer").on('click', function () {
        let firstName = $(this).data("first-name");
        let lastName = $(this).data("last-name");
        let cell = $(this).data("cell");
        let createdDate = $(this).data("created-date");
        let alwaysInclude = $(this).data("always-include");
        let id = $(this).data("id");
       

        $("#contributor_first_name").val(firstName);
        $("#contributor_last_name").val(lastName);
        $("#contributor_cell_number").val(cell);
        $("#contributor_created_at").val(createdDate);
        $("#contributor_always_include").prop('checked', alwaysInclude == "True");
        $("#initialDepositDiv").remove();
       
        $("#addModal").modal()
        let form = $(".add-contributor");
        form.attr('action', '/Contributors/edit');
        $("#contributor-id").val(id);

    })
})
