using ControleFerias.Data;
using ControleFerias.Enums;
using ControleFerias.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;


namespace ControleFerias.Models
{
    public class Ferias
    {
        [Key]
        public int Id { get; set; }

        [Required, JsonConverter(typeof(DateTimeJsonConverter))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dDataInicio { get; set; }

        [Column(TypeName = "text")]
        [Required(ErrorMessage = "A quantidade de dias é necessária")]
        public int sDias { get; set; }

        [Required, JsonConverter(typeof(DateTimeJsonConverter))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dDataFinal { get; set; }

        
        public StatusFerias Status { get; set; } = StatusFerias.Pendente;
        public string? sComentario { get; set; }


        public ICollection<ColaboradorFerias>? ColaboradorFerias { get; set; }

        public ICollection<Colaborador>? Colaborador { get; set; }

        public Ferias() { }
        public Ferias(DateTime ddatainicio, int dias, DateTime ddatafinal, StatusFerias status, string comentario, int colaboradorId, ApplicationDBContext context)
        {
            ValidateDomain(ddatainicio, dias, ddatafinal, status, comentario, colaboradorId,context);
        }

        public void Update(DateTime ddataInicio, int dias, DateTime ddatafinal, StatusFerias status, string comentario, int colaboradorId, ApplicationDBContext context)
        {
            ValidateDomain(ddataInicio, dias, ddatafinal, status, comentario, colaboradorId,context);
        }
        private void ValidateDomain(DateTime ddatainicio, int dias, DateTime ddatafinal, StatusFerias status, string comentario, int colaboradorId, ApplicationDBContext context)
        {
            comentario = comentario?.Trim() ?? string.Empty;


            DomainExceptionValidation.When((ddatafinal <= ddatainicio),"A data final deve ser maior que a data inicial ");

            DomainExceptionValidation.When((ddatainicio <= DateTime.Today),"A data inicial deve ser maior que a data atual ");

            DomainExceptionValidation.When((dias <= 0),"A quantidade de dias deve ser maior que zero.");

            DomainExceptionValidation.When(dias != (ddatafinal - ddatainicio).Days,"A quantidade de dias não corresponde ao período entre data de início e final.");

            DomainExceptionValidation.When(status != StatusFerias.Pendente,"O status inicial de uma nova férias deve ser Pendente.");

            DomainExceptionValidation.When(comentario.Length > 200,"O comentário não pode ter mais de 200 caracteres.");

            DomainExceptionValidation.When(ddatainicio.DayOfWeek == DayOfWeek.Saturday || ddatainicio.DayOfWeek == DayOfWeek.Sunday,"A data inicial não pode ser sábado ou domingo.");

            bool existeSobreposicao = context.Ferias
       .Where(f => f.ColaboradorFerias.Any(cf => cf.ColaboradorId == colaboradorId))
       .Any(f => dDataInicio <= f.dDataFinal && dDataFinal >= f.dDataInicio);



            this.dDataInicio = ddatainicio;
            this.sDias = dias;
            this.dDataFinal = ddatafinal;
        }

        //Alterar o formato de data para dd/MM/yyyy
        public class DateTimeJsonConverter : JsonConverter<DateTime>
        {
            private readonly string _format = "dd/MM/yyyy";

            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.ParseExact(reader.GetString()!, _format, null);
            }
            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString(_format));
            }
        }
    }
}
