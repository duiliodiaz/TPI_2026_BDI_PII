using System;
using System.Collections.Generic;
using CineApp.Back.Models;
using Microsoft.EntityFrameworkCore;

namespace CineApp.Back.Data;

public partial class CineDbContext : DbContext
{
    public CineDbContext()
    {
    }

    public CineDbContext(DbContextOptions<CineDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actore> Actores { get; set; }

    public virtual DbSet<Butaca> Butacas { get; set; }

    public virtual DbSet<Clasificacione> Clasificaciones { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Comprobante> Comprobantes { get; set; }

    public virtual DbSet<DetallesComprobanteProducto> DetallesComprobanteProductos { get; set; }

    public virtual DbSet<DetallesPeliculaActor> DetallesPeliculaActors { get; set; }

    public virtual DbSet<DetallesReservaEntradum> DetallesReservaEntrada { get; set; }

    public virtual DbSet<Directore> Directores { get; set; }

    public virtual DbSet<Entrada> Entradas { get; set; }

    public virtual DbSet<EstadosReserva> EstadosReservas { get; set; }

    public virtual DbSet<FormasPago> FormasPagos { get; set; }

    public virtual DbSet<Funcione> Funciones { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<Pelicula> Peliculas { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Promocione> Promociones { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Sala> Salas { get; set; }

    public virtual DbSet<TamañosProducto> TamañosProductos { get; set; }

    public virtual DbSet<TiposDescuento> TiposDescuentos { get; set; }

    public virtual DbSet<TiposProducto> TiposProductos { get; set; }

    public virtual DbSet<TiposSala> TiposSalas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VwResumenMensual> VwResumenMensuals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=FELIPOLLO\\SQLEXPRESS;Database=CINE_2026_TPI_01;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actore>(entity =>
        {
            entity.HasKey(e => e.IdActor).HasName("pk_actores");

            entity.ToTable("actores");

            entity.Property(e => e.IdActor).HasColumnName("id_actor");
            entity.Property(e => e.Apellido)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.FecNacimiento)
                .HasColumnType("datetime")
                .HasColumnName("fec_nacimiento");
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nacionalidad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Butaca>(entity =>
        {
            entity.HasKey(e => e.IdButaca).HasName("pk_butacas");

            entity.ToTable("butacas");

            entity.Property(e => e.IdButaca).HasColumnName("id_butaca");
            entity.Property(e => e.Columna).HasColumnName("columna");
            entity.Property(e => e.Fila)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("fila");
            entity.Property(e => e.IdSala).HasColumnName("id_sala");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Butacas)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_butacas_salas");
        });

        modelBuilder.Entity<Clasificacione>(entity =>
        {
            entity.HasKey(e => e.IdClasificacion).HasName("pk_clasificaciones");

            entity.ToTable("clasificaciones");

            entity.Property(e => e.IdClasificacion).HasColumnName("id_clasificacion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("pk_clientes");

            entity.ToTable("clientes");

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Dni)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.EMail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("e_mail");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Comprobante>(entity =>
        {
            entity.HasKey(e => e.IdComprobante).HasName("pk_comprobantes");

            entity.ToTable("comprobantes");

            entity.Property(e => e.IdComprobante).HasColumnName("id_comprobante");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdFormaPago).HasColumnName("id_forma_pago");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Comprobantes)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comprobantes_clientes");

            entity.HasOne(d => d.IdFormaPagoNavigation).WithMany(p => p.Comprobantes)
                .HasForeignKey(d => d.IdFormaPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comprobantes_formas_pago");
        });

        modelBuilder.Entity<DetallesComprobanteProducto>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("pk_detalles_comprobante_prod");

            entity.ToTable("detalles_comprobante_productos");

            entity.Property(e => e.IdDetalle).HasColumnName("id_detalle");
            entity.Property(e => e.CantidadProductos).HasColumnName("cantidad_productos");
            entity.Property(e => e.IdComprobante).HasColumnName("id_comprobante");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdComprobanteNavigation).WithMany(p => p.DetallesComprobanteProductos)
                .HasForeignKey(d => d.IdComprobante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_cp_comprobantes");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetallesComprobanteProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_cp_productos");
        });

        modelBuilder.Entity<DetallesPeliculaActor>(entity =>
        {
            entity.HasKey(e => e.IdPeliculaActor).HasName("pk_detalles_pelicula_actor");

            entity.ToTable("detalles_pelicula_actor");

            entity.Property(e => e.IdPeliculaActor).HasColumnName("id_pelicula_actor");
            entity.Property(e => e.IdActor).HasColumnName("id_actor");
            entity.Property(e => e.IdPelicula).HasColumnName("id_pelicula");

            entity.HasOne(d => d.IdActorNavigation).WithMany(p => p.DetallesPeliculaActors)
                .HasForeignKey(d => d.IdActor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_pa_actores");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.DetallesPeliculaActors)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_pa_peliculas");
        });

        modelBuilder.Entity<DetallesReservaEntradum>(entity =>
        {
            entity.HasKey(e => e.IdDetalleReservas).HasName("pk_detalles_reserva_entrada");

            entity.ToTable("detalles_reserva_entrada");

            entity.Property(e => e.IdDetalleReservas).HasColumnName("id_detalle_reservas");
            entity.Property(e => e.IdEntrada).HasColumnName("id_entrada");
            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");

            entity.HasOne(d => d.IdEntradaNavigation).WithMany(p => p.DetallesReservaEntrada)
                .HasForeignKey(d => d.IdEntrada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_re_entradas");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.DetallesReservaEntrada)
                .HasForeignKey(d => d.IdReserva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_re_reservas");
        });

        modelBuilder.Entity<Directore>(entity =>
        {
            entity.HasKey(e => e.IdDirector).HasName("pk_directores");

            entity.ToTable("directores");

            entity.Property(e => e.IdDirector).HasColumnName("id_director");
            entity.Property(e => e.Apellido)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.FecNacimiento)
                .HasColumnType("datetime")
                .HasColumnName("fec_nacimiento");
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nacionalidad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Entrada>(entity =>
        {
            entity.HasKey(e => e.IdEntrada).HasName("pk_entradas");

            entity.ToTable("entradas");

            entity.Property(e => e.IdEntrada).HasColumnName("id_entrada");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.IdButaca).HasColumnName("id_butaca");
            entity.Property(e => e.IdComprobante).HasColumnName("id_comprobante");
            entity.Property(e => e.IdFuncion).HasColumnName("id_funcion");
            entity.Property(e => e.IdPromocion).HasColumnName("id_promocion");
            entity.Property(e => e.PrecioCobrado)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_cobrado");

            entity.HasOne(d => d.IdButacaNavigation).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.IdButaca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_entradas_butacas");

            entity.HasOne(d => d.IdComprobanteNavigation).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.IdComprobante)
                .HasConstraintName("fk_entradas_comprobantes");

            entity.HasOne(d => d.IdFuncionNavigation).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.IdFuncion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_entradas_funciones");

            entity.HasOne(d => d.IdPromocionNavigation).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.IdPromocion)
                .HasConstraintName("fk_entradas_promociones");
        });

        modelBuilder.Entity<EstadosReserva>(entity =>
        {
            entity.HasKey(e => e.IdEstadoReserva).HasName("pk_estados_reserva");

            entity.ToTable("estados_reserva");

            entity.Property(e => e.IdEstadoReserva).HasColumnName("id_estado_reserva");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<FormasPago>(entity =>
        {
            entity.HasKey(e => e.IdFormaPago).HasName("pk_formas_pago");

            entity.ToTable("formas_pago");

            entity.Property(e => e.IdFormaPago).HasColumnName("id_forma_pago");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Funcione>(entity =>
        {
            entity.HasKey(e => e.IdFuncion).HasName("pk_funciones");

            entity.ToTable("funciones");

            entity.Property(e => e.IdFuncion).HasColumnName("id_funcion");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdPelicula).HasColumnName("id_pelicula");
            entity.Property(e => e.IdSala).HasColumnName("id_sala");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.Funciones)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_funciones_peliculas");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Funciones)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_funciones_salas");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGenero).HasName("pk_generos");

            entity.ToTable("generos");

            entity.Property(e => e.IdGenero).HasColumnName("id_genero");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.HasKey(e => e.IdPelicula).HasName("pk_peliculas");

            entity.ToTable("peliculas");

            entity.Property(e => e.IdPelicula).HasColumnName("id_pelicula");
            entity.Property(e => e.Duracion).HasColumnName("duracion");
            entity.Property(e => e.FechaEstreno)
                .HasColumnType("datetime")
                .HasColumnName("fecha_estreno");
            entity.Property(e => e.IdClasificacion).HasColumnName("id_clasificacion");
            entity.Property(e => e.IdDirector).HasColumnName("id_director");
            entity.Property(e => e.IdGenero).HasColumnName("id_genero");
            entity.Property(e => e.Sinopsis)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("sinopsis");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdClasificacionNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdClasificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_peliculas_clasificaciones");

            entity.HasOne(d => d.IdDirectorNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdDirector)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_peliculas_directores");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_peliculas_generos");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("pk_productos");

            entity.ToTable("productos");

            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdTamañoProducto).HasColumnName("id_tamaño_producto");
            entity.Property(e => e.IdTipoProducto).HasColumnName("id_tipo_producto");
            entity.Property(e => e.PreUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("pre_unitario");

            entity.HasOne(d => d.IdTamañoProductoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdTamañoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productos_tamaños");

            entity.HasOne(d => d.IdTipoProductoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdTipoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productos_tipos");
        });

        modelBuilder.Entity<Promocione>(entity =>
        {
            entity.HasKey(e => e.IdPromocion).HasName("pk_promociones");

            entity.ToTable("promociones");

            entity.Property(e => e.IdPromocion).HasColumnName("id_promocion");
            entity.Property(e => e.Dia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dia");
            entity.Property(e => e.HoraFin).HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdTipoDescuento).HasColumnName("id_tipo_descuento");

            entity.HasOne(d => d.IdTipoDescuentoNavigation).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.IdTipoDescuento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_promociones_tipos_desc");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("pk_reservas");

            entity.ToTable("reservas");

            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");
            entity.Property(e => e.FechaReserva)
                .HasColumnType("datetime")
                .HasColumnName("fecha_reserva");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_reservas_clientes");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_reservas_estados");
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.IdSala).HasName("pk_salas");

            entity.ToTable("salas");

            entity.Property(e => e.IdSala).HasColumnName("id_sala");
            entity.Property(e => e.Capacidad).HasColumnName("capacidad");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdTipoSala).HasColumnName("id_tipo_sala");

            entity.HasOne(d => d.IdTipoSalaNavigation).WithMany(p => p.Salas)
                .HasForeignKey(d => d.IdTipoSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_salas_tipos");
        });

        modelBuilder.Entity<TamañosProducto>(entity =>
        {
            entity.HasKey(e => e.IdTamañoProducto).HasName("pk_tamaños_producto");

            entity.ToTable("tamaños_producto");

            entity.Property(e => e.IdTamañoProducto).HasColumnName("id_tamaño_producto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<TiposDescuento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDescuento).HasName("pk_tipos_descuento");

            entity.ToTable("tipos_descuento");

            entity.Property(e => e.IdTipoDescuento).HasColumnName("id_tipo_descuento");
            entity.Property(e => e.Descripcion)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<TiposProducto>(entity =>
        {
            entity.HasKey(e => e.IdTipoProducto).HasName("pk_tipos_producto");

            entity.ToTable("tipos_producto");

            entity.Property(e => e.IdTipoProducto).HasColumnName("id_tipo_producto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<TiposSala>(entity =>
        {
            entity.HasKey(e => e.IdTipoSala).HasName("pk_tipos_sala");

            entity.ToTable("tipos_sala");

            entity.Property(e => e.IdTipoSala).HasColumnName("id_tipo_sala");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.PrecioEntrada)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_entrada");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("pk_usuarios");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.NombreUsuario, "uq_usuarios_nombre").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Empleado")
                .HasColumnName("rol");
        });

        modelBuilder.Entity<VwResumenMensual>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_resumen_mensual");

            entity.Property(e => e.CantidadTotalDeEntradasVendidas).HasColumnName("Cantidad Total de Entradas Vendidas");
            entity.Property(e => e.Género)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Película)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RecaudaciónTotal)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Recaudación Total");
            entity.Property(e => e.TipoDeSala)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tipo de sala");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
