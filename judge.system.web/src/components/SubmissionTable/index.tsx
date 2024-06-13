
import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import { useCookies } from "react-cookie";
import { toast } from "react-toastify";
import styles from "@/styles/Submissions.module.css";
import { jwtDecode } from "jwt-decode";
// const Submissions: React.FC<SubmissionsProps> = ({ userName }) =>
export type Submission = {
  time: string;
  problemTitle: string;
  isAccepted: boolean;
  numCasesPassed: string;
  language: string;
};
type SubmitionTableProps = {

}
const SubmitionTable: React.FC<SubmitionTableProps> = () => {
  const [submissionList, setSubmissionList] = useState<Submission[]>([]);
  //const [user, setUser] = useState<any>();
  const router = useRouter();
  const [cookie] = useCookies(["token"]);
    let user: any = undefined;
  // useEffect(() => {
  //   if (cookie.token != null) {
  //     setUser(jwtDecode(cookie.token));
  //   }
  // }, [cookie.token]);
  useEffect(() => {
    const fetchProblemDetails = async () => {
      if (!cookie.token) {
        toast.warn("Please login to access your submission", {
          position: "top-center",
          autoClose: 3000,
          theme: "dark",
        });
        return router.push("/auth");
      }
      user = jwtDecode(cookie.token);
      try {
        const response = await fetch(
          `http://localhost:5107/api/submissions?userName=${user.nameid}`,
          {
            method: "GET",
            mode: "cors",
            headers: {
              Authorization: `Bearer ${cookie.token}`,
              "Content-Type": "application/json;charset=UTF-8",
              Accept: "application/json, text/plain",
            },
          }
        );
        const data = await response.json();
        setSubmissionList(data.data);
      } catch (error) {
        console.log(error);
        toast.error("Failed to fetch submission details");
      }
    };

    fetchProblemDetails();
  }, []);

  console.log(submissionList);

  return (
    <table
      className={styles.table}
      style={{ maxWidth: "calc(100% - 30px)", margin: "auto" }}
    >
      <thead>
        <tr>
          <th className={styles.timeSubmitted}>Time Submitted</th>
          <th className={styles.tableTitle}>Title</th>
          <th className={styles.status}>Status</th>
          <th className={styles.numCasesPassed}>Num Cases Passed</th>
          <th className={styles.language}>Language</th>
        </tr>
      </thead>
      <tbody>
        {submissionList.map((submission, index) => (
          <tr
            key={index}
            className={index % 2 === 0 ? styles.evenRow : styles.oddRow}
          >
            <td>{submission.time}</td>
            <td className={styles.problemTitle}>{submission.problemTitle}</td>
            <td
              className={
                submission.isAccepted ? styles.accepted : styles["wrong-answer"]
              }
            >
              {submission.isAccepted ? "Accepted" : "Wrong Answer"}
            </td>
            <td>{submission.numCasesPassed}</td>
            <td>{submission.language}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};
export default SubmitionTable;
