import React, { useEffect, useState } from 'react';
import styles from '../styles/Submissions.module.css';
import Topbar from '@/components/Topbar/Topbar';
import { toast } from "react-toastify";

export type Submission = {
  time: string;
  problemTitle: string;
  isAccepted: boolean;
  numCasesPassed: string;
  language: string;
};

interface SubmissionsProps {
  userName: string;
}

const Submissions: React.FC<SubmissionsProps> = ({ userName }) => {
  const [submissionList, setSubmissionList] = useState<Submission[]>([]);

  useEffect(() => {
    const fetchProblemDetails = async () => {
      try {
        const response = await fetch(
          `http://localhost:5107/api/submissions?userName=${userName}`, 
          {
            method: "GET",
            mode: "cors",
            headers: {
              Accept: "application/json, text/plain",
              "Content-Type": "application/json;charset=UTF-8",
            },
          }
        );
        const data = await response.json();
        setSubmissionList(data.data);
      } catch (error) {
        toast.error("Failed to fetch problem details");
      }
    };

    fetchProblemDetails();
  }, [userName]);

  console.log(submissionList);

  return (
    <div className="bg-dark-layer-2 min-h-screen">
      <Topbar />
      <h1 className={styles.title}>All My Submissions</h1>
      <table className={styles.table}>
        <thead>
          <tr>
            <th>Time Submitted</th>
            <th>Title</th>
            <th>Status</th>
            <th>Num Cases Passed</th>
            <th>Language</th>
          </tr>
        </thead>
        <tbody>
          {submissionList.map((submission, index) => (
            <tr key={index} className={index % 2 === 0 ? styles.evenRow : styles.oddRow}>
              <td>{submission.time}</td>
              <td className={styles.question}>{submission.problemTitle}</td>
              <td className={
                submission.isAccepted ? styles.accepted :
                styles['wrong-answer']
              }>
                {submission.isAccepted ? 'Accepted' : 'Wrong Answer'}
              </td>
              <td>{submission.numCasesPassed}</td>
              <td>{submission.language}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default Submissions;
