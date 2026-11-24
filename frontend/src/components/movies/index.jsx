import Movie from '../movie'

export default function Movies ({ movies }) {
    if (movies.length === 0) {
        return <p className="text-secondary">No hay películas disponibles.</p>;
    }
    return (
        <>
            <section aria-label="Películas en cartelera">
                <ul className="row g-5 list-unstyled mb-0 justify-content-center">
                    {movies.map((movie) => (
                        <li key={movie.id} className="col-6 col-md-4 col-lg-3">
                            <Movie {...movie} />
                        </li>
                    ))}
                </ul>
            </section>
        </>
    )
}