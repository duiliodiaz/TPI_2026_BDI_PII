import styles from "./Movie.module.css";


const ratingColors = {
  G: "#4b5160",
  "R-13": "#1d8fe1",
  "R-16": "#f59e0b",
  "R-18": "#dc2626",
};

function formatDuration(minutes) {
  const h = Math.floor(minutes / 60);
  const m = minutes % 60;
  return {
    label: `${h}h${String(m).padStart(2, "0")}m`,
    iso: `PT${h}H${m}M`,
  };
}

export default function Movie({
  title,
  poster,
  minutes,
  rating,
  formats,
  href = "#",
}) {
  const duration = formatDuration(minutes);
  return (
    <article className={`card ${styles.card} h-100 overflow-hidden shadow`}>
      <figure className={`${styles.poster} position-relative m-0`}>
        <img src={poster} alt={`Póster de ${title}`} loading="lazy" />
        <figcaption className="visually-hidden">{title}</figcaption>
        <time className={styles.duration} dateTime={duration.iso}>
          {duration.label}
        </time>
      </figure>

      <div className={`card-body ${styles.body} px-4 pt-0 pb-4`}>
        <header className="d-flex align-items-start justify-content-between gap-2 mb-3">
          <h3 className={styles.title}>
            <a href={href} className="stretched-link">
              {title}
            </a>
          </h3>
          <span
            className={styles.rating}
            style={{ "--rating-bg": ratingColors[rating] }}
            title={`Clasificación ${rating}`}
          >
            {rating}
          </span>
        </header>

        <ul className={`${styles.formats} list-unstyled d-flex flex-wrap mb-0`}>
          {formats.map((format) => (
            <li key={format}>{format}</li>
          ))}
        </ul>
      </div>
    </article>
  );
}

/* Uso (en una fila, para que las cards tengan la misma altura):
<div className="row g-4">
  <div className="col-6 col-lg-3">
    <MovieCard
      title="Cyber Noir: Protocolo Cero"
      poster={cyberNoir}
      minutes={134}
      rating="R-13"
      formats={["4D E-Motion", "2D", "Comfort", "XD", "D-Box"]}
      href="/peliculas/cyber-noir"
    />
  </div>
  <div className="col-6 col-lg-3">
    <MovieCard
      title="Lumina: El viaje mágico"
      poster={lumina}
      minutes={102}
      rating="G"
      formats={["3D", "D-Box", "4D E-Motion", "2D"]}
      href="/peliculas/lumina"
    />
  </div>
</div>
*/
