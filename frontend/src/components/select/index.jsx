import styles from "./Select.module.css";
import IconDown from "../../assets/IconDown.svg"

export default function Select({ className = "", children, ...props }) {
  return (
    <div className={styles.wrap}>
      <select className={`${styles.select} form-control ${className}`} {...props}>
        {children}
      </select>
      <img src={IconDown} className={`${styles.chevron}`} aria-hidden="true" />
    </div>
  );
}