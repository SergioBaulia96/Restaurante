window.onload = ListadosPlatos();

function ListadosPlatos() {

    $.ajax({
        url: '../../Menus/ListadosPlatos',
        data: {
        },
        type: 'POST',
        dataType: 'json',
        success: function (menusMostrar) {

            let contenidoTabla = ``;

            $.each(menusMostrar, function (index, menu) {

                contenidoTabla += `
                <tr>
                    <td>${menu.nombre}</td>
                    <td colspan="4"></td>
                </tr>
            `;

                $.each(menu.listadoPlatos, function (index, plato) {
                let precioFormateado = `$ ${parseFloat(plato.precio).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
                    contenidoTabla += `
                        <tr>
                            <td></td>
                            <td>${plato.nombrePlato}</td>
                            <td class="d-none d-md-table-cell">${plato.descripcion}</td>
                            <td>${precioFormateado}</td>
                        </tr>
                    `;


                });

            });

            document.getElementById("tbody-listadoPlatos").innerHTML = contenidoTabla;

        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al cargar el listado');
        }
    });
}

function ImprimirListado() {
    // var doc = new jsPDF();
    var doc = new jsPDF("l", "mm", [297, 210]);
  
    var totalPagesExp = "{total_pages_count_string}";
    var pageContent = function (data) {
      var pageHeight =
        doc.internal.pageSize.height || doc.internal.pageSize.getHeight();
      var pageWidth =
        doc.internal.pageSize.width || doc.internal.pageSize.getWidth();
  
      // // HEADER
      // doc.setFontSize(14);
      // doc.setFont("helvetica", "bold");
      // doc.text("Informe de publicaciones por fecha", 15, 10);
  
      // FOOTER
      var str = "Pagina " + data.pageCount;
      // Total page number plugin only available in jspdf v1.0+
      if (typeof doc.putTotalPages == "function") {
        str = str + " de " + totalPagesExp;
      }
  
      doc.setLineWidth(8);
      doc.setDrawColor(238, 238, 238);
      doc.line(14, pageHeight - 11, 196, pageHeight - 11);
  
      doc.setFontSize(10);
  
      doc.setFontStyle("bold");
  
      doc.text(str, 17, pageHeight - 10);
    };
  
    var elem = document.getElementById("ListadoPlatos");
    var res = doc.autoTableHtmlToJson(elem);
    doc.autoTable(res.columns, res.data, {
      addPageContent: pageContent,
      theme: "grid",
      styles: { fillColor: [0, 143, 81], halign: "center" },
      columnStyles: 
        {
          0: { halign: "center", fillColor: [255, 255, 255], fontSize: 7 },
          1: { halign: "left", fillColor: [255, 255, 255], fontSize: 7 },
          2: { halign: "left", fillColor: [255, 255, 255], fontSize: 7 },
          3: { halign: "left", fillColor: [255, 255, 255], fontSize: 7 },
          4: { halign: "left", fillColor: [255, 255, 255], fontSize: 7, cellWidth: 30 },
          5: { halign: "right", fillColor: [255, 255, 255], fontSize: 7 },
          6: { halign: "left", fillColor: [255, 255, 255], fontSize: 7 },
          7: { halign: "left", fillColor: [255, 255, 255], fontSize: 7 },
        },
      margin: { top: 10 },
    });
  
    // ESTO SE LLAMA ANTES DE ABRIR EL PDF PARA QUE MUESTRE EN EL PDF EL NRO TOTAL DE PAGINAS. ACA CALCULA EL TOTAL DE PAGINAS.
    if (typeof doc.putTotalPages === "function") {
      doc.putTotalPages(totalPagesExp);
    }
  
    //doc.save('InformeSistema.pdf')
  
    var string = doc.output("datauristring");
    var iframe =
      "<iframe width='100%' height='100%' src='" + string + "'></iframe>";
  
    var x = window.open();
    x.document.open();
    x.document.write(iframe);
    x.document.close();
  }