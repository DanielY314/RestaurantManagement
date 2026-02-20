import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const styles = {
  container: {
    display: 'flex', justifyContent: 'center', alignItems: 'center',
    height: '100vh', backgroundColor: '#1a1a2e',
  },
  box: {
    backgroundColor: '#16213e', padding: '40px', borderRadius: '12px',
    width: '340px', boxShadow: '0 8px 32px rgba(0,0,0,0.4)',
  },
  title: { color: '#e2b04a', textAlign: 'center', marginBottom: '8px', fontSize: '24px' },
  subtitle: { color: '#aaa', textAlign: 'center', marginBottom: '28px', fontSize: '14px' },
  label: { color: '#ccc', fontSize: '13px', marginBottom: '6px', display: 'block' },
  input: {
    width: '100%', padding: '10px 12px', borderRadius: '6px',
    border: '1px solid #333', backgroundColor: '#0f3460', color: '#fff',
    fontSize: '14px', marginBottom: '16px', boxSizing: 'border-box',
  },
  button: {
    width: '100%', padding: '12px', backgroundColor: '#e2b04a', color: '#1a1a2e',
    border: 'none', borderRadius: '6px', fontSize: '15px', fontWeight: 'bold',
    cursor: 'pointer', marginTop: '8px',
  },
  error: { color: '#ff6b6b', fontSize: '13px', marginBottom: '12px', textAlign: 'center' },
};

function AdminLogin() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const res = await fetch(`${process.env.REACT_APP_API_URL}/User/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password }),
      });
      if (!res.ok) { setError('이메일 또는 비밀번호가 틀렸습니다.'); setLoading(false); return; }
      const data = await res.json();
      localStorage.setItem('adminToken', data.token);
      navigate('/admin');
    } catch {
      setError('서버에 연결할 수 없습니다.');
    }
    setLoading(false);
  };

  return (
    <div style={styles.container}>
      <div style={styles.box}>
        <h2 style={styles.title}>관리자 로그인</h2>
        <p style={styles.subtitle}>Restaurant POS System</p>
        <form onSubmit={handleLogin}>
          <label style={styles.label}>이메일</label>
          <input style={styles.input} type="email" value={email}
            onChange={e => setEmail(e.target.value)} required />
          <label style={styles.label}>비밀번호</label>
          <input style={styles.input} type="password" value={password}
            onChange={e => setPassword(e.target.value)} required />
          {error && <p style={styles.error}>{error}</p>}
          <button style={styles.button} type="submit" disabled={loading}>
            {loading ? '로그인 중...' : '로그인'}
          </button>
        </form>
      </div>
    </div>
  );
}

export default AdminLogin;
