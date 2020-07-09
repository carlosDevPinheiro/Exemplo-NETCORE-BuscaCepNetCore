using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace BuscaCEPConsole.Models
{
    public class CepModel
    {
        [Key]
        public int Id { get; set; }

        [JsonProperty("cep")]
        public string Cep { get; set; }

        [JsonProperty("logradouro")]
        public string Logradouro { get; set; }

        [JsonProperty("complemento")]
        public string Complemento { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("localidade")]
        public string Localidade { get; set; }

        [JsonProperty("uf")]
        public string UF { get; set; }

        [JsonProperty("unidade")]
        public string Unidade { get; set; }

        [JsonProperty("ibge")]
        public string Ibge { get; set; }

        [JsonProperty("gia")]
        public string Gia { get; set; }
    }

    public class CepModelConfigurations: IEntityTypeConfiguration<CepModel>
    {
        public void Configure(EntityTypeBuilder<CepModel> builder)
        {
            builder.ToTable("CEP");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Bairro).HasColumnName("Bairro");
            builder.Property(c => c.Logradouro).HasColumnName("Logradouro");
            builder.Property(c => c.Complemento).HasColumnName("Complemento");
            builder.Property(c => c.Localidade).HasColumnName("Cidade");
            builder.Property(c => c.Unidade).HasColumnName("Unidade");
            builder.Property(c => c.Ibge).HasColumnName("IBGE");
            builder.Property(c => c.Gia).HasColumnName("Gia");
            builder.Property(c => c.Cep).HasColumnName("CEP");
            builder.Property(c => c.UF).HasColumnName("Estado");
        }
    }

}