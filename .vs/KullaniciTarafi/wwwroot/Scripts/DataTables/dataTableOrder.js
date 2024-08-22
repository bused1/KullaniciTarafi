// Code goes here

 $(function(){
     $.extend( $.fn.dataTableExt.oSort, {
	"turkish-pre": function ( a ) {
		var special_letters = {
            "C": "Ca", "c": "ca", "Ç": "Cb", "ç": "cb",
            "G": "Ga", "g": "ga", "Ğ": "Gb", "ğ": "gb",
            "I": "Ia", "ı": "ia", "İ": "Ib", "i": "ib",
            "O": "Oa", "o": "oa", "Ö": "Ob", "ö": "ob",
            "S": "Sa", "s": "sa", "Ş": "Sb", "ş": "sb",
            "U": "Ua", "u": "ua", "Ü": "Ub", "ü": "ub"
            };
        for (var val in special_letters)
           a = a.split(val).join(special_letters[val]).toLowerCase();
        return a;
	},

	"turkish-asc": function ( a, b ) {
		return ((a < b) ? -1 : ((a > b) ? 1 : 0));
	},

	"turkish-desc": function ( a, b ) {
		return ((a < b) ? 1 : ((a > b) ? -1 : 0));
	}
} );
     var dataSet1 = [
          ['A ','A '],
          ['Ğ ','B'],
          ['Z','C'],
          ['Ç','B']
          
     ];
     $('#datatable-table').dataTable( {
         'aoColumns' : [
                      {'sType' : 'turkish'},
                      {'sType' : 'string'}
      ],
          "destroy":true, //To delete previous DataTable instance
          "data": dataSet1, //Link the dataset
          "iDisplayLength": 10, //Number of dataitems in one page
          "bLengthChange": false, //Change this to 'true', to choose number of items per page
          "columns": [ // Table column names
               { "title": "Student Name" },
               { "title": "Student Grade" }
          ],
     }); 
     
});

