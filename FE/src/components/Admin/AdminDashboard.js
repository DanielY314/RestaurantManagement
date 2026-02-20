import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const API = process.env.REACT_APP_API_URL;

const s = {
  page: { display: 'flex', height: '100vh', backgroundColor: '#1a1a2e', color: '#fff', fontFamily: 'sans-serif' },
  sidebar: { width: '200px', backgroundColor: '#16213e', padding: '24px 16px', flexShrink: 0 },
  logo: { color: '#e2b04a', fontSize: '18px', fontWeight: 'bold', marginBottom: '32px' },
  navBtn: { display: 'block', width: '100%', padding: '10px 14px', marginBottom: '8px', backgroundColor: 'transparent',
    color: '#ccc', border: 'none', borderRadius: '6px', textAlign: 'left', cursor: 'pointer', fontSize: '14px' },
  navBtnActive: { backgroundColor: '#0f3460', color: '#e2b04a' },
  logoutBtn: { display: 'block', width: '100%', padding: '10px 14px', marginTop: 'auto',
    backgroundColor: '#c0392b', color: '#fff', border: 'none', borderRadius: '6px',
    cursor: 'pointer', fontSize: '14px', position: 'absolute', bottom: '24px', width: '168px' },
  main: { flex: 1, padding: '28px', overflowY: 'auto' },
  title: { color: '#e2b04a', fontSize: '22px', marginBottom: '20px' },
  table: { width: '100%', borderCollapse: 'collapse', fontSize: '14px' },
  th: { backgroundColor: '#0f3460', padding: '10px 14px', textAlign: 'left', color: '#e2b04a' },
  td: { padding: '10px 14px', borderBottom: '1px solid #222' },
  rowHover: { backgroundColor: '#16213e', cursor: 'pointer' },
  badge: { padding: '3px 10px', borderRadius: '12px', fontSize: '12px', backgroundColor: '#e2b04a', color: '#1a1a2e' },
  modal: { position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(0,0,0,0.7)',
    display: 'flex', justifyContent: 'center', alignItems: 'center', zIndex: 100 },
  modalBox: { backgroundColor: '#16213e', padding: '28px', borderRadius: '12px', minWidth: '400px', maxWidth: '560px' },
  modalTitle: { color: '#e2b04a', marginBottom: '16px', fontSize: '18px' },
  closeBtn: { marginTop: '16px', padding: '8px 20px', backgroundColor: '#333', color: '#fff',
    border: 'none', borderRadius: '6px', cursor: 'pointer' },
};

function OrdersTab({ token }) {
  const [orders, setOrders] = useState([]);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [items, setItems] = useState([]);
  const [foods, setFoods] = useState([]);

  useEffect(() => {
    fetch(`${API}/FoodOrder`, { headers: { Authorization: `Bearer ${token}` } })
      .then(r => r.json()).then(setOrders).catch(() => {});
    fetch(`${API}/Food`)
      .then(r => r.json()).then(setFoods).catch(() => {});
  }, [token]);

  const openOrder = async (order) => {
    setSelectedOrder(order);
    const res = await fetch(`${API}/FoodOrderItem/${order.id}`, {
      headers: { Authorization: `Bearer ${token}` }
    });
    const data = await res.json();
    setItems(data);
  };

  const getFoodName = (foodId) => {
    const food = foods.find(f => f.id === foodId);
    return food ? food.name : `음식 #${foodId}`;
  };

  const sorted = [...orders].sort((a, b) => new Date(b.order_date) - new Date(a.order_date));

  return (
    <div>
      <h2 style={s.title}>주문 목록</h2>
      <table style={s.table}>
        <thead>
          <tr>
            <th style={s.th}>주문 번호</th>
            <th style={s.th}>테이블</th>
            <th style={s.th}>날짜/시간</th>
            <th style={s.th}>합계</th>
            <th style={s.th}>상세</th>
          </tr>
        </thead>
        <tbody>
          {sorted.map(order => (
            <tr key={order.id} style={s.rowHover}>
              <td style={s.td}>#{order.id}</td>
              <td style={s.td}>{order.seatName || '-'}</td>
              <td style={s.td}>{new Date(order.order_date).toLocaleString('ko-KR')}</td>
              <td style={s.td}>₩{Number(order.total).toLocaleString()}</td>
              <td style={s.td}>
                <button style={s.badge} onClick={() => openOrder(order)}>보기</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {selectedOrder && (
        <div style={s.modal} onClick={() => setSelectedOrder(null)}>
          <div style={s.modalBox} onClick={e => e.stopPropagation()}>
            <h3 style={s.modalTitle}>주문 #{selectedOrder.id} 상세 — 테이블: {selectedOrder.seatName}</h3>
            <table style={s.table}>
              <thead>
                <tr>
                  <th style={s.th}>메뉴</th>
                  <th style={s.th}>수량</th>
                </tr>
              </thead>
              <tbody>
                {items.map(item => (
                  <tr key={item.id}>
                    <td style={s.td}>{getFoodName(item.food_id)}</td>
                    <td style={s.td}>{item.count}개</td>
                  </tr>
                ))}
              </tbody>
            </table>
            <p style={{ color: '#e2b04a', marginTop: '12px' }}>합계: ₩{Number(selectedOrder.total).toLocaleString()}</p>
            <button style={s.closeBtn} onClick={() => setSelectedOrder(null)}>닫기</button>
          </div>
        </div>
      )}
    </div>
  );
}

function MenuTab() {
  const [foods, setFoods] = useState([]);
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    fetch(`${API}/Food`).then(r => r.json()).then(setFoods).catch(() => {});
    fetch(`${API}/Category`).then(r => r.json()).then(setCategories).catch(() => {});
  }, []);

  const getCategoryName = (id) => {
    const c = categories.find(c => c.categoryId === id);
    return c ? c.name : '-';
  };

  return (
    <div>
      <h2 style={s.title}>메뉴 목록</h2>
      <table style={s.table}>
        <thead>
          <tr>
            <th style={s.th}>이름</th>
            <th style={s.th}>카테고리</th>
            <th style={s.th}>가격</th>
          </tr>
        </thead>
        <tbody>
          {foods.map(food => (
            <tr key={food.id}>
              <td style={s.td}>{food.name}</td>
              <td style={s.td}>{getCategoryName(food.categoryId)}</td>
              <td style={s.td}>₩{Number(food.sales_price).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function AdminDashboard() {
  const [tab, setTab] = useState('orders');
  const navigate = useNavigate();
  const token = localStorage.getItem('adminToken');

  const logout = () => {
    localStorage.removeItem('adminToken');
    navigate('/admin/login');
  };

  return (
    <div style={s.page}>
      <div style={{ ...s.sidebar, position: 'relative' }}>
        <div style={s.logo}>🍽 Restaurant POS</div>
        <button style={{ ...s.navBtn, ...(tab === 'orders' ? s.navBtnActive : {}) }}
          onClick={() => setTab('orders')}>주문 관리</button>
        <button style={{ ...s.navBtn, ...(tab === 'menu' ? s.navBtnActive : {}) }}
          onClick={() => setTab('menu')}>메뉴 보기</button>
        <button style={s.logoutBtn} onClick={logout}>로그아웃</button>
      </div>
      <div style={s.main}>
        {tab === 'orders' && <OrdersTab token={token} />}
        {tab === 'menu' && <MenuTab />}
      </div>
    </div>
  );
}

export default AdminDashboard;
