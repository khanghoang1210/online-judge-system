import React from 'react';
import styles from '../styles/Submissions.module.css';
import Topbar from '@/components/Topbar/Topbar';

const submissions = [
  { timeSubmitted: '5 ngày, 19 giờ trước', question: 'Chuỗi con dài nhất không có ký tự lặp lại', status: 'Wrong answer', runtime: 'N/A', language: 'javascript' },
  { timeSubmitted: '5 ngày, 19 giờ trước', question: 'Hai số', status: 'Wrong answer', runtime: 'N/A', language: 'javascript' },
  { timeSubmitted: '5 ngày, 19 giờ trước', question: 'Hai số', status: 'Wrong answer', runtime: 'N/A', language: 'javascript' },
  { timeSubmitted: '3 tuần, 5 ngày trước', question: 'Hai số', status: 'Accepted', runtime: '2110 ms', language: 'python' },
  { timeSubmitted: '3 tuần, 5 ngày trước', question: 'Chứa nhiều nước nhất', status: 'Runtime Error', runtime: 'N/A', language: 'python' },
  { timeSubmitted: '8 tháng, 3 tuần trước', question: 'Kết hợp hai bảng', status: 'Accepted', runtime: '1113 ms', language: 'mysql' },
  { timeSubmitted: '1 năm, 2 tháng trước', question: 'Duyệt cây nhị phân theo thứ tự giữa', status: 'Accepted', runtime: '32 ms', language: 'python3' },
  { timeSubmitted: '1 năm, 2 tháng trước', question: 'Kết hợp hai danh sách đã sắp xếp', status: 'Accepted', runtime: '39 ms', language: 'python3' },

];

const Submissions: React.FC = () => {
  return (
    <div className={styles.submissions}>
      <Topbar/>
      <h1 className={styles.title}>All My Submissions</h1>
      <table className={styles.table}>
        <thead>
          <tr>
            <th>Time Submitted</th>
            <th>Question</th>
            <th>Status</th>
            <th>Runtime</th>
            <th>Language</th>
          </tr>
        </thead>
        <tbody>
          {submissions.map((submission, index) => (
            <tr key={index} className={index % 2 === 0 ? styles.evenRow : styles.oddRow}>
              <td>{submission.timeSubmitted}</td>
              <td className={styles.question}>{submission.question}</td>
              <td className={
                submission.status === 'Accepted' ? styles.accepted :
                submission.status === 'Wrong answer' ? styles['wrong-answer'] :
                styles['runtime-error']
              }>
                {submission.status}
              </td>
              <td>{submission.runtime}</td>
              <td>{submission.language}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default Submissions;
